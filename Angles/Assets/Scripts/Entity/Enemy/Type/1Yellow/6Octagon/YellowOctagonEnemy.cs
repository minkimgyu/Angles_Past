using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowOctagonEnemy : DelayFollowEnemy
{
    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance, BuffFloat skillCooldownTime,
        BuffFloat followDistance, BuffFloat followOffsetDistance, BuffFloat attackDelay, BuffInt directionCount)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, skillUseDistance,
            skillUseOffsetDistance, skillCooldownTime, followDistance, followOffsetDistance, attackDelay);

        this.directionCount = directionCount;
    }

    //[SerializeField]
    //GameObject[] expectationDirs;
    //public GameObject[] ExpectationDirs { get { return expectationDirs; } }

    [SerializeField]
    BuffInt directionCount; // 개수만 정해줌
    public BuffInt DirectionCount { get { return directionCount; } }

    protected override void AddState()
    {
        BaseState<State> attack = new StateYellowOctagonAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
