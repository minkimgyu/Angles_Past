using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSquareEnemy : BaseFollowEnemy
{
    protected override void AddState()
    {
        BaseState<State> attack = new StateYellowSquareAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }
}
