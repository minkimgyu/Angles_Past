using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Enemy<T, W> : StateMachineEntity<T, W>, IHealth
{
    //public FollowComponent followComponent;
    //public BasicReflectComponent basicReflectComponent;
    //public ForceComponent forceComponent;

    //private Player m_loadPlayer;
    //public Player LoadPlayer { get { return m_loadPlayer; } }

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

    //private FollowComponent m_followComponent;
    //public FollowComponent FollowComponent { get { return m_followComponent; } }

    //DashComponent m_dashComponent;
    //public DashComponent DashComponent { get { return m_dashComponent; } }

    //MoveComponent m_moveComponent;
    //public MoveComponent MoveComponent { get { return m_moveComponent; } }

    //BattleComponent m_battleComponent;
    //public BattleComponent BattleComponent { get { return m_battleComponent; } }

    private Rigidbody2D m_rigid;
    public Rigidbody2D Rigid { get { return m_rigid; } }

    //public enum State
    //{
    //    Attack,
    //    Follow,
    //    Stop,
    //    Die
    //}

    //스테이트들을 보관
    //private Dictionary<State, IState<Enemy>> m_dicState = new Dictionary<State, IState<Enemy>>();

    protected virtual void Awake()
    {
        //m_followComponent = GetComponent<FollowComponent>();
        //m_dashComponent = GetComponent<DashComponent>();
        //m_moveComponent = GetComponent<MoveComponent>();
        m_rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(EnemyData enemyData)
    {
        m_data = enemyData;
        //hp = enemyData.Hp;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //m_loadPlayer = PlayManager.Instance.Player;

        ////상태 생성
        //IState<Enemy> follow = new StateEnemyFollow(); // 적마다 state machine만 다르게 해서 적용시키기
        //IState<Enemy> attack = new StateEnemyAttack();
        //IState<Enemy> stop = new StateEnemyStop();
        //IState<Enemy> die = new StateEnemyDie();

        ////키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        //m_dicState.Add(State.Attack, attack);
        //m_dicState.Add(State.Follow, follow);
        //m_dicState.Add(State.Stop, stop);
        //m_dicState.Add(State.Die, die);

        ////기본상태는 달리기로 설정.
        //m_stateMachine = new StateMachine<Enemy>(this, follow);

        //m_stateMachine.SetGlobalState(attack);
    }

    public bool IsTarget(EntityTag tag)
    {
        throw new NotImplementedException();
    }

    public void UnderAttack(float healthPoint)
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

    abstract public void WhenUnderAttack(); // ---> 색상 변화 or 이펙트 적용

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


    //public override void KnockBack(float knockBackThrust)
    //{
    //    throw new NotImplementedException();
    //}

    //private void OnDisable()
    //{
    //    ObjectPooler.ReturnToPool(gameObject);
    //}

    //public override void GetHit(float damage, Vector3 dir)
    //{
    //    base.GetHit(damage);
    //    followComponent.WaitFollow();
    //    basicReflectComponent.KnockBack(dir);
    //}

    //protected override void Die()
    //{
    //    if(dieAction != null) dieAction();

    //    GameObject go = ObjectPooler.SpawnFromPool(dieEffect, transform.position, transform.rotation);
    //    go.GetComponent<DieEffect>().Init(enemyData.Color);
    //    base.Die();
    //}
}