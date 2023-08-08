using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BasicConstruction : StateMachineEntity<BasicConstruction.State>
{
    public Action ContactAction;

    public enum State
    {
        Global,
        Idle,
        Attack,

        Pull,
        Push
    }

    BattleComponent m_battleComponent;
    public BattleComponent BattleComponent { get { return m_battleComponent; } }

    ContactComponent m_contactComponent;
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    [SerializeField]
    protected GrantedUtilization grantedUtilization;

    protected virtual void Awake()
    {
        m_battleComponent = GetComponent<BattleComponent>();
        m_contactComponent = GetComponent<ContactComponent>();
    }

    public override void InitData()
    {
    }

    public abstract void Init();

    private void Update()
    {
        DoOperateUpdate();
    }

    private void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        ContactComponent.CallWhenCollisionEnter(col);

        if (ContactAction != null) ContactAction();
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        ContactComponent.CallWhenCollisionExit(col);
    }
}

public class StateBasicConstructionIdle : BaseState<BasicConstruction.State>
{
    BasicConstruction loadBasicConstruction;

    public StateBasicConstructionIdle(BasicConstruction basicConstruction)
    {
        loadBasicConstruction = basicConstruction;
    }

    public override void OnMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public override void OperateEnter()
    {
    }

    public override void OperateExit()
    {
    }

    public override void ReceiveCollisionEnter(Collision2D collision) 
    {
        loadBasicConstruction.SetState(BasicConstruction.State.Attack);
    }

    public override void OperateUpdate()
    {
    }
}

public class StateBasicConstructionAttack : BaseState<BasicConstruction.State>
{
    BasicConstruction loadBasicConstruction;

    public StateBasicConstructionAttack(BasicConstruction basicConstruction)
    {
        loadBasicConstruction = basicConstruction;
    }
    public override void OnMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        loadBasicConstruction.BattleComponent.UseSkill(SkillUseConditionType.Contact);
        loadBasicConstruction.RevertToPreviousState();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}