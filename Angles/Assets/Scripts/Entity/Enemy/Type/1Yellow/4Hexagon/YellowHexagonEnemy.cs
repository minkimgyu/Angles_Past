using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowHexagonEnemy : BaseFollowEnemy
{
    [SerializeField]
    float delay; // ���� ��ų ���������� ������
    public float Delay { get { return delay; } }

    [SerializeField]
    float fixTime; // ������ �ð��� �ð�
    public float FixTime { get { return fixTime; } }

    protected override void Init()
    {
        base.Init();
        BaseState<State> attack = new StateYellowHexaagonAttack(this); // ���ݸ� ���� �߰�������
        BaseState<State> fix = new StateYellowHexaagonFix(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.Fix, fix);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
