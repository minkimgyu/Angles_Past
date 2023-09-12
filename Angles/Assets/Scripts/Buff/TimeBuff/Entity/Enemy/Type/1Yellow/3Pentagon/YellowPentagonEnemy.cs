using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowPentagonEnemy : DelayFollowEnemy
{
    protected override void AddState()
    {
        BaseState<State> attack = new StateYellowPentagonAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }
}
