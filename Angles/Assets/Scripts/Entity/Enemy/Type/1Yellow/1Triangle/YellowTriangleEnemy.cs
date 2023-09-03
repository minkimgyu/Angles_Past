using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class YellowTriangleEnemy : BaseFollowEnemy
{
    [HideInInspector]
    public string skillEffectName;

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance,
        BuffFloat followDistance, BuffFloat followOffsetDistance, string skillEffectName)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance);

        this.skillEffectName = skillEffectName;
    }

    BasicEffectPlayer effectPlayer; // ���ο� ������ ����Ʈ �÷��̾�
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } } // ���ο� ������ ����Ʈ �÷��̾�

    protected override void AddState()
    {
        BaseState<State> follow = new StateTriangleEnemyFollow(this);
        BaseState<State> stop = new StateTriangleEnemyStop(this);
        BaseState<State> die = new StateFollowEnemyDie(this);
        BaseState<State> damaged = new StateFollowEnemyDamaged(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Follow, follow); // �⺻ ���°� ���� --> �Ÿ��� ���� ���� or ������
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
        m_dicState.Add(State.Damaged, damaged);

        BaseState<State> attack = new StateYellowTriangleAttack(this); // ���ݸ� ���� �߰������� --> GlobalState�� ��ųʸ��� ���� ���� ��������
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }
}
