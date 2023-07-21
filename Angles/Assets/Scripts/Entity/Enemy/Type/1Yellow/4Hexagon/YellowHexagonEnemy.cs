using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowHexagonEnemy : BaseFollowEnemy
{
    [SerializeField]
    float delay;
    public float Delay { get { return delay; } }

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateYellowHexaagonAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
