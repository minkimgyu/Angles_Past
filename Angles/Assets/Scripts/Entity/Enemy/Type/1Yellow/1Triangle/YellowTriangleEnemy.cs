using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowTriangleEnemy : BaseFollowEnemy
{
    [SerializeField]
    EffectMethod effectMethod; // 이팩트 불러오기
    public EffectMethod EffectMethod { get { return effectMethod; } } // 이팩트 불러오기


    BasicEffectPlayer effectPlayer; // 슬로우 묻히는 이펙트 플레이어
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } } // 슬로우 묻히는 이펙트 플레이어

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateYellowTriangleAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
