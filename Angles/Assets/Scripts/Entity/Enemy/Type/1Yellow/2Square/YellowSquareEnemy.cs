using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSquareEnemy : BaseFollowEnemy
{
    protected override void AddState()
    {
        BaseState<State> attack = new StateYellowSquareAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }
}
