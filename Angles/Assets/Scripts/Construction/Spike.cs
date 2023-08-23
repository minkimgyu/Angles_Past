using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : BasicConstruction
{
    public override void Init()
    {
        BaseState<State> idle = new StateBasicConstructionIdle(this);
        BaseState<State> attack = new StateBasicConstructionAttack(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Idle, idle);
        m_dicState.Add(State.Attack, attack);

        SetUp(State.Idle);
    }
}
