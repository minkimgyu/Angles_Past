using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        IState<State> attack = new StateYellowTriangleAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
