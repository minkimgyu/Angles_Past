using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSquareEnemy : BaseFollowEnemy
{


    protected override void Start()
    {
        
    }

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateYellowSquareAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
