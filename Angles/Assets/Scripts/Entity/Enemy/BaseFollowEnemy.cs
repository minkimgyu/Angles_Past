using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class DelayFollowEnemy : BaseFollowEnemy
{
    /// <summary>
    /// 다음 스킬 전까지 딜레이
    /// </summary>
    BuffFloat attackDelay;
    public BuffFloat AttackDelay { get { return attackDelay; } }

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance,BuffFloat followDistance, 
        BuffFloat followOffsetDistance, BuffFloat attackDelay)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance);

        this.attackDelay = attackDelay.CopyData();
    }
}

abstract public class BaseFollowEnemy : Enemy<BaseFollowEnemy.State>
{
    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames, 
        BuffInt score, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance, BuffFloat followDistance,   
        BuffFloat followOffsetDistance)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score);

        this.skillUseDistance = skillUseDistance.CopyData();
        this.skillUseOffsetDistance = skillUseOffsetDistance.CopyData();
        this.followDistance = followDistance.CopyData();
        this.followOffsetDistance = followOffsetDistance.CopyData();

        m_loadPlayer = PlayManager.Instance.PlayerTransform; // 오브젝트 풀링에서 꺼낼 때, 초기화 시켜주기
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

    protected Transform m_loadPlayer;
    public Transform LoadPlayer { get { return m_loadPlayer; } }

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

    //protected override void Start()
    //{
    //    base.Start();
    //    FindPlayer();
    //}

    //protected virtual void FindPlayer()
    //{
    //    GameObject go = GameObject.FindWithTag("PlayerTransform");
    //    if (go == null) return;

    //    m_loadPlayer = go.GetComponent<PlayerTransform>();
    //}

    protected virtual void AddBaseState()
    {
        BaseState<State> follow = new StateFollowEnemyFollow(this);
        BaseState<State> stop = new StateFollowEnemyStop(this);
        BaseState<State> die = new StateFollowEnemyDie(this);
        BaseState<State> damaged = new StateFollowEnemyDamaged(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Follow, follow); // 기본 상태가 추적 --> 거리에 따라 정지 or 움직임
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
        m_dicState.Add(State.Damaged, damaged);
    }
}
