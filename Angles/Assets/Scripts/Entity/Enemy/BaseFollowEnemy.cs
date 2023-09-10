using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class DelayFollowEnemy : BaseFollowEnemy
{
    /// <summary>
    /// ���� ��ų ������ ������
    /// </summary>
    BuffFloat attackDelay;
    public BuffFloat AttackDelay { get { return attackDelay; } }

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffInt goldCount, BuffFloat spawnPercentage, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance,BuffFloat followDistance, 
        BuffFloat followOffsetDistance, BuffFloat attackDelay)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance);

        this.attackDelay = attackDelay.CopyData();
    }
}

abstract public class BaseFollowEnemy : Enemy<BaseFollowEnemy.State>
{
    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames, 
        BuffInt score, BuffInt goldCount, BuffFloat spawnPercentage, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance, BuffFloat followDistance,   
        BuffFloat followOffsetDistance)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage);

        this.skillUseDistance = skillUseDistance.CopyData();
        this.skillUseOffsetDistance = skillUseOffsetDistance.CopyData();
        this.followDistance = followDistance.CopyData();
        this.followOffsetDistance = followOffsetDistance.CopyData();

        m_loadPlayer = PlayManager.Instance.Player; // ������Ʈ Ǯ������ ���� ��, �ʱ�ȭ �����ֱ�
        m_followComponent.Initialize(followDistance.IntervalValue,
            followDistance.IntervalValue + followOffsetDistance.IntervalValue);
    }


    BuffFloat skillUseDistance;
    public BuffFloat SkillUseDistance { get { return skillUseDistance; } set { skillUseDistance = value; } }

    BuffFloat skillUseOffsetDistance;
    public BuffFloat SkillUseOffsetDistance { get { return skillUseOffsetDistance; } set { skillUseOffsetDistance = value; } }

    BuffFloat skillCooldownTime;
    public BuffFloat SkillReuseTime { get { return skillCooldownTime; } set { skillCooldownTime = value; } }

    BuffFloat followDistance;
    public BuffFloat FollowDistance { get { return followDistance; } set { followDistance = value; } }

    BuffFloat followOffsetDistance;
    public BuffFloat FollowOffsetDistance { get { return followOffsetDistance; } set { followOffsetDistance = value; } }


    //protected StorageSO storageSO;
    //public StorageSO StorageSO { get { return storageSO; } }

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
        Global,
    }

    protected override void Awake()
    {
        base.Awake();
        m_followComponent = GetComponent<FollowComponent>();
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
