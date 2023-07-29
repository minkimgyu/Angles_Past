using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseFollowEnemy : Enemy<BaseFollowEnemy.State>
{
    public Action WhenEnable;

    protected Player m_loadPlayer;
    public Player LoadPlayer { get { return m_loadPlayer; } }

    private FollowComponent m_followComponent;
    public FollowComponent FollowComponent { get { return m_followComponent; } }

    MoveComponent m_moveComponent;
    public MoveComponent MoveComponent { get { return m_moveComponent; } }

    BattleComponent m_battleComponent;
    public BattleComponent BattleComponent { get { return m_battleComponent; } }

    DashComponent m_dashComponent;
    public DashComponent DashComponent { get { return m_dashComponent; } }

    ContactComponent m_contactComponent;
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    public enum State
    {
        Attack,
        Follow,
        Stop,
        Die,
        Fix,
        Damaged,
    }

    protected override void Awake()
    {
        base.Awake();
        m_followComponent = GetComponent<FollowComponent>();
        m_moveComponent = GetComponent<MoveComponent>();
        m_battleComponent = GetComponent<BattleComponent>();
        m_dashComponent = GetComponent<DashComponent>();
        m_contactComponent = GetComponent<ContactComponent>();


        //m_battleComponent.AbleTags.Add(EntityTag.Player);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        GameObject go = GameObject.FindWithTag("Player");
        if(go != null)
        {
            m_loadPlayer = go.GetComponent<Player>();
        }
        

        IState<State> follow = new StateFollowEnemyFollow(this);
        IState<State> stop = new StateFollowEnemyStop(this);
        IState<State> die = new StateFollowEnemyDie(this);
        IState<State> damaged = new StateFollowEnemyDamaged(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Follow, follow); // �⺻ ���°� ���� --> �Ÿ��� ���� ���� or ������
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
        m_dicState.Add(State.Damaged, damaged);

        Data.GrantedUtilization.LootSkillFromDB(BattleComponent);
    }

    private void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ
    {
        m_contactComponent.CallWhenCollisionEnter(col);

        BattleComponent.UseSkill(SkillUseConditionType.Contact);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        m_contactComponent.CallWhenCollisionExit(col);
    }

    protected virtual void OnEnable()
    {
        if (WhenEnable != null)
        {
            WhenEnable();
        }
    }

    protected virtual void OnDestroy()
    {
        if (WhenEnable != null)
        {
            WhenEnable = null;
        }
    }
}
