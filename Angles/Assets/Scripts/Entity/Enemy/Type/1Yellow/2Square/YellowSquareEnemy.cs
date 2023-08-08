using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSquareEnemy : BaseFollowEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Init()
    {
        base.Init();
        BaseState<State> attack = new StateYellowSquareAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(attack);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
