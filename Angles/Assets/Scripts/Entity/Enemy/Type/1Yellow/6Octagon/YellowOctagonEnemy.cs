using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowOctagonEnemy : BaseFollowEnemy
{
    [SerializeField]
    GameObject[] expectationDirs;
    public GameObject[] ExpectationDirs { get { return expectationDirs; } }

    [SerializeField]
    float delay;
    public float Delay { get { return delay; } }

    [SerializeField]
    int directionCount;
    public int DirectionCount { get { return directionCount; } }

    protected override void Init()
    {
        base.Init();
        BaseState<State> attack = new StateYellowOctagonAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
