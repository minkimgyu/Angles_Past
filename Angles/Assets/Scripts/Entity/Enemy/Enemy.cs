using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class Enemy<T> : Avatar<T>
{
    protected BuffInt score;
    public BuffInt Score { get { return score; }}

    protected virtual void Start() => AddState();

    protected abstract void AddState();

<<<<<<< Updated upstream
    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
    BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames, BuffInt score)
=======
    float originHp;
    bool crackOnce = false;

    [SerializeField]
    protected Sprite crackSprite;

    [SerializeField]
    protected EffectMethod dieEffectMethod;

    SpriteRenderer m_spriteRenderer;

    public EnemyData m_data;

    public EnemyData Data
>>>>>>> Stashed changes
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames);

<<<<<<< Updated upstream
        this.score = score.CopyData();
=======
            m_rigid.mass = m_data.Weight;
            m_rigid.drag = m_data.Drag;
        }
    }

    private Rigidbody2D m_rigid;
    public Rigidbody2D Rigid { get { return m_rigid; } }

    public Action<float, Vector2, float> UnderAttackAction;

    //string[] dropSkills = { "GhostItem", "BarrierItem", "BladeItem", "KnockbackItem", "ShockwaveItem", "SpawnBallItem", "SpawnGravityBallItem", "StickyBombItem" };

    protected virtual void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigid = GetComponent<Rigidbody2D>();
        //m_rigid.mass = m_data.Weight;
        //m_rigid.drag = m_data.Drag;
    }

    public override void InitData()
    {
        m_data = DatabaseManager.Instance.EntityDB.ReturnEnemyData(name);

        originHp = m_data.Hp;
        crackOnce = false;
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

            if(originHp / 2 >= m_data.Hp && crackOnce == false) // 절반보다 작을 경우
            {
                // 이미지 변경 출력
                m_spriteRenderer.sprite = crackSprite;
                crackOnce = true;
            }

            if (m_data.Hp <= 0)
            {
                SpawnGold();
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
            string name = PlayManager.instance.dropSkills[UnityEngine.Random.Range(0, PlayManager.instance.dropSkills.Length)];
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

    void SpawnGold()
    {
        for (int i = 0; i < m_data.GoldCount; i++)
        {
            float x = Random.Range(-1.5f, 1.5f);
            float y = Random.Range(-1.5f, 1.5f);

            Gold gold = ObjectPooler.SpawnFromPool<Gold>("Gold");
            gold.transform.position = transform.position + new Vector3(x, y);
        }
    }

    protected virtual void OnDisable()
    {
        if (UnderAttackAction != null) UnderAttackAction = null;
        ObjectPooler.ReturnToPool(gameObject);
>>>>>>> Stashed changes
    }
}