using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriangleEnemy : BaseFollowEnemy
{
    TriggerComponent m_triggerComponent;
    public TriggerComponent TriggerComponent 
    {
        get { return m_triggerComponent; }
    }

    protected override void Awake()
    {
        base.Awake();
        m_triggerComponent = GetComponentInChildren<TriggerComponent>();
    }

    protected override void AddState()
    {
        BaseState<State> attack = new StateBossTriangleEnemyAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }
}
