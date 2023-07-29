using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class YellowTriangleEnemy : BaseFollowEnemy
{
    public Action WhenDisable;

    [SerializeField]
    EffectMethod effectMethod; // ����Ʈ �ҷ�����
    public EffectMethod EffectMethod { get { return effectMethod; } } // ����Ʈ �ҷ�����


    BasicEffectPlayer effectPlayer; // ���ο� ������ ����Ʈ �÷��̾�
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } } // ���ο� ������ ����Ʈ �÷��̾�

    protected override void Init()
    {
        m_loadPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();

        IState<State> follow = new StateTriangleEnemyFollow(this);
        IState<State> stop = new StateTriangleEnemyStop(this);
        IState<State> die = new StateFollowEnemyDie(this);
        IState<State> damaged = new StateFollowEnemyDamaged(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Follow, follow); // �⺻ ���°� ���� --> �Ÿ��� ���� ���� or ������
        m_dicState.Add(State.Stop, stop);
        m_dicState.Add(State.Die, die);
        m_dicState.Add(State.Damaged, damaged);

        Data.GrantedUtilization.LootSkillFromDB(BattleComponent);

        IState<State> attack = new StateYellowTriangleAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);
    }

    protected override void OnDisable()
    {
        if(WhenDisable != null)
        {
            WhenDisable();
        }

        if (effectPlayer != null)
        {
            effectPlayer.StopEffect();
            effectPlayer = null;
        }

        base.OnDisable();
    }

    protected override void OnDestroy()
    {
        if (WhenDisable != null)
        {
            WhenDisable = null;
        }

        base.OnDestroy();
    }
}
