using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        BaseState<State> follow = new StateTriangleEnemyFollow(this);
        BaseState<State> stop = new StateTriangleEnemyStop(this);
        BaseState<State> die = new StateFollowEnemyDie(this);
        BaseState<State> damaged = new StateFollowEnemyDamaged(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Follow, follow); // 기본 상태가 추적 --> 거리에 따라 정지 or 움직임
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
        m_dicState.Add(State.Damaged, damaged);

        BaseState<State> attack = new StateYellowTriangleAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
