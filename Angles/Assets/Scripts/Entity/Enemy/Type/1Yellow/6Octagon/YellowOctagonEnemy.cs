using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowOctagonEnemy : BaseFollowEnemy
{
    [SerializeField]
    float delay;
    public float Delay { get { return delay; } }

    [SerializeField]
    int directionCount;
    public int DirectionCount { get { return directionCount; } }

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateYellowOctagonAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}