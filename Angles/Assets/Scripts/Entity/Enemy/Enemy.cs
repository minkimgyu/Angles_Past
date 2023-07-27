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

    protected BuffComponent buffComponent;
    [SerializeField]
    public BuffComponent BuffComponent { get { return buffComponent; } }


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

    protected virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(EnemyData enemyData)
    {
        m_data = enemyData;
        //hp = enemyData.Hp;
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
        //print("UnderAttack");

        if (m_data.Hp > 0)
        {
            m_data.Hp -= healthPoint;
            WhenUnderAttack();

            if (m_data.Hp <= 0)
            {
                Die();
                m_data.Hp = 0;
            }
        }
    }

    public void WhenUnderAttack() // ---> 색상 변화 or 이펙트 적용
    {
    }

    public void Heal(float healthPoint)
    {
        throw new NotImplementedException();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }

    public EntityTag ReturnTag()
    {
        return inheritedTag;
    }

    public void Knockback(Vector2 dir, float thrust)
    {
        //print("Knockback");
    }

    protected virtual void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}