using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class YellowTriangleEnemy : BaseFollowEnemy
{
    [SerializeField]
    EffectMethod effectMethod; // ����Ʈ �ҷ�����
    public EffectMethod EffectMethod { get { return effectMethod; } } // ����Ʈ �ҷ�����


    BasicEffectPlayer effectPlayer; // ���ο� ������ ����Ʈ �÷��̾�
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } } // ���ο� ������ ����Ʈ �÷��̾�

    protected override void Init()
    {
        base.Init();

        BaseState<State> follow = new StateTriangleEnemyFollow(this);
        BaseState<State> stop = new StateTriangleEnemyStop(this);
        BaseState<State> die = new StateFollowEnemyDie(this);
        BaseState<State> damaged = new StateFollowEnemyDamaged(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Follow, follow); // �⺻ ���°� ���� --> �Ÿ��� ���� ���� or ������
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
        m_dicState.Add(State.Damaged, damaged);

        BaseState<State> attack = new StateYellowTriangleAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
