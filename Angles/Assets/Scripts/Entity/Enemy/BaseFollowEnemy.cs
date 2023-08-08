using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseFollowEnemy : Enemy<BaseFollowEnemy.State>
{
    protected FollowEnemyData m_followEnemyData;
    public FollowEnemyData FollowEnemyData { get { return m_followEnemyData; } }


    protected Player m_loadPlayer;
    public Player LoadPlayer { get { return m_loadPlayer; } }

    private FollowComponent m_followComponent;
    public FollowComponent FollowComponent { get { return m_followComponent; } }

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
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        GameObject go = GameObject.FindWithTag("Player");
        if (go == null) return;

        m_loadPlayer = go.GetComponent<Player>();
    }

    protected virtual void AddBaseState()
    {
        BaseState<State> follow = new StateFollowEnemyFollow(this);
        BaseState<State> stop = new StateFollowEnemyStop(this);
        BaseState<State> die = new StateFollowEnemyDie(this);
        BaseState<State> damaged = new StateFollowEnemyDamaged(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Follow, follow); // �⺻ ���°� ���� --> �Ÿ��� ���� ���� or ������
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
        m_dicState.Add(State.Damaged, damaged);
    }
}
