using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFollowEnemy : Enemy<BaseFollowEnemy, BaseFollowEnemy.State>
{
    private Player m_loadPlayer;
    public Player LoadPlayer { get { return m_loadPlayer; } }

    private FollowComponent m_followComponent;
    public FollowComponent FollowComponent { get { return m_followComponent; } }

    MoveComponent m_moveComponent;
    public MoveComponent MoveComponent { get { return m_moveComponent; } }

    BattleComponent m_battleComponent;
    public BattleComponent BattleComponent { get { return m_battleComponent; } }

    public enum State
    {
        Attack,
        Follow,
        Stop,
        Die
    }

    protected override void Awake()
    {
        base.Awake();
        m_followComponent = GetComponent<FollowComponent>();
        m_moveComponent = GetComponent<MoveComponent>();
        m_battleComponent = GetComponent<BattleComponent>();

        m_battleComponent.AbleTags.Add(EntityTag.Player);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_loadPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();

        IState<BaseFollowEnemy, State> follow = new StateFollowEnemyFollow();
        IState<BaseFollowEnemy, State> stop = new StateFollowEnemyStop();
        IState<BaseFollowEnemy, State> die = new StateFollowEnemyDie();
        IState<BaseFollowEnemy, State> attack = new StateFollowEnemyAttack();

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Follow, follow); // �⺻ ���°� ���� --> �Ÿ��� ���� ���� or ������
        m_dicState.Add(State.Stop, stop); 
        m_dicState.Add(State.Die, die); 
        m_dicState.Add(State.Attack, attack); // ��������� ����

        SetUp(this, State.Follow);
        SetGlobalState(attack);
    }

    // Update is called once per frame
    void Update()
    {
        DoOperateUpdate();
    }

    public override void WhenUnderAttack()
    {
        throw new System.NotImplementedException();
    }
}
