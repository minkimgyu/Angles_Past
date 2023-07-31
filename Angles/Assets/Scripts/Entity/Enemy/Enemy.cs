using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Enemy<T> : StateMachineEntity<T>, IHealth
{
    //public FollowComponent followComponent;
    //public BasicReflectComponent basicReflectComponent;
    //public ForceComponent forceComponent;

    //private Player m_loadPlayer;
    //public Player LoadPlayer { get { return m_loadPlayer; } }

    //[SerializeField]
    //protected BuffComponent buffComponent;
    //public BuffComponent BuffComponent { get { return buffComponent; } }

    [SerializeField]
    protected EffectMethod dieEffectMethod;

    public EnemyData m_data;

    public EnemyData Data
    {
        get
        {
            return m_data;
        }
        set
        {
            m_data = value;
            if (m_rigid == null) return;

            m_rigid.mass = m_data.Weight;
            m_rigid.drag = m_data.Drag;
        }
    }

    private Rigidbody2D m_rigid;
    public Rigidbody2D Rigid { get { return m_rigid; } }

    public Action<float, Vector2, float> UnderAttackAction;

    string[] dropSkills = { "GhostItem", "BarrierItem", "BladeItem", "KnockbackItem", "ShockwaveItem", "SpawnBallItem", "SpawnGravityBallItem", "StickyBombItem" };

    protected virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        //m_rigid.mass = m_data.Weight;
        //m_rigid.drag = m_data.Drag;
    }

    public override void InitData()
    {
        m_data = DatabaseManager.Instance.EntityDB.ReturnEnemyData(name);
    }

    // Update is called once per frame
    void Update()
    {
        DoOperateUpdate();
    }

    public bool IsTarget(EntityTag tag)
    {
        throw new NotImplementedException();
    }

    public void UnderAttack(float healthPoint, Vector2 dir, float thrust)
    {
        if (m_data.Hp > 0)
        {
            SoundManager.Instance.PlaySFX(transform.position, "Hit", 0.7f);
            m_data.Hp -= healthPoint;
            if (m_data.Hp <= 0)
            {
                Die();
                m_data.Hp = 0;
            }
        }

        if (UnderAttackAction != null) UnderAttackAction(healthPoint, dir, thrust);
    }

    public void WhenUnderAttack() // ---> 색상 변화 or 이펙트 적용
    {
    }

    public void Heal(float healthPoint)
    {
        throw new NotImplementedException();
    }

    void ShowDieEffect()
    {
        BasicEffectPlayer effectPlayer = dieEffectMethod.ReturnEffectFromPool();
        if (effectPlayer == null) return;

        effectPlayer.Init(transform.position, 2f);
        effectPlayer.PlayEffect();
    }

    void SpawnRandomItem()
    {
        float percentage = UnityEngine.Random.Range(0.0f, 1.0f);

        if(percentage <= m_data.SpawnPercentage)
        {
            string name = dropSkills[UnityEngine.Random.Range(0, dropSkills.Length)];
            DropSkill skill = ObjectPooler.SpawnFromPool<DropSkill>(name);
            skill.transform.position = transform.position;
        }
    }

    public virtual void Die()
    {
        SoundManager.Instance.PlaySFX(transform.position, "Die", 0.3f);

        ShowDieEffect();

        PlayManager.instance.ScoreUp(m_data.Score);

        SpawnRandomItem();
        gameObject.SetActive(false);
    }

    public EntityTag ReturnTag()
    {
        return inheritedTag;
    }

    protected virtual void OnDisable()
    {
        if (UnderAttackAction != null) UnderAttackAction = null;

        ObjectPooler.ReturnToPool(gameObject);
    }
}