using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : BasicConstruction
{
    public override void Init()
    {
        BaseState<State> idle = new StateBasicConstructionIdle(this);
        BaseState<State> attack = new StateBasicConstructionAttack(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Idle, idle);
        m_dicState.Add(State.Attack, attack);

        SetUp(State.Idle);
    }
}
