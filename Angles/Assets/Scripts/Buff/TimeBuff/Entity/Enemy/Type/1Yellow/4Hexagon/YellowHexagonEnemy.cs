using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class YellowHexagonEnemy : DelayFollowEnemy
{
    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffInt goldCount, BuffFloat spawnPercentage, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance,
        BuffFloat followDistance, BuffFloat followOffsetDistance, BuffFloat attackDelay, BuffFloat fixTime)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance, attackDelay);

        this.fixTime = fixTime.CopyData();
    }

    [SerializeField]
    BuffFloat fixTime; // ������ �ð�
    public BuffFloat FixTime { get { return fixTime; } }

    protected override void AddState()
    {
        BaseState<State> attack = new StateYellowHexaagonAttack(this); // ���ݸ� ���� �߰�������
        BaseState<State> fix = new StateYellowHexaagonFix(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.Fix, fix);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }
}
