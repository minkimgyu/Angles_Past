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
        base.Init();
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
