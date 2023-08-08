using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowHexagonEnemy : BaseFollowEnemy
{
    [SerializeField]
    float delay; // 다음 스킬 시전까지의 딜레이
    public float Delay { get { return delay; } }

    [SerializeField]
    float fixTime; // 고정된 시간의 시간
    public float FixTime { get { return fixTime; } }

    protected override void Init()
    {
        base.Init();
        BaseState<State> attack = new StateYellowHexaagonAttack(this); // 공격만 따로 추가해주자
        BaseState<State> fix = new StateYellowHexaagonFix(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.Fix, fix);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
