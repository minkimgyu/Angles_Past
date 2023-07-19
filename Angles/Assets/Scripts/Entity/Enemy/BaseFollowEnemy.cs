using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFollowEnemy : Enemy<BaseFollowEnemy.State>
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


        //m_battleComponent.AbleTags.Add(EntityTag.Player);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        m_loadPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();

        IState<State> follow = new StateFollowEnemyFollow(this);
        IState<State> stop = new StateFollowEnemyStop(this);
        IState<State> die = new StateFollowEnemyDie(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Follow, follow); // �⺻ ���°� ���� --> �Ÿ��� ���� ���� or ������
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
    }

    public override void WhenUnderAttack()
    {
        throw new System.NotImplementedException();
    }
}
