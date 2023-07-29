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

public class StateBasicConstructionIdle : IState<BasicConstruction.State>
{
    BasicConstruction loadBasicConstruction;

    public StateBasicConstructionIdle(BasicConstruction basicConstruction)
    {
        loadBasicConstruction = basicConstruction;
    }

    public void CheckSwitchStates()
    {
    }

    public void OnAwakeMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public void OnProcessingMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        loadBasicConstruction.ContactAction += GoToAttackState;
    }

    public void OperateExit()
    {
        loadBasicConstruction.ContactAction -= GoToAttackState;
    }

    void GoToAttackState()
    {
        loadBasicConstruction.SetState(BasicConstruction.State.Attack);
    }

    public void OperateUpdate()
    {
    }
}

public class StateBasicConstructionAttack : IState<BasicConstruction.State>
{
    BasicConstruction loadBasicConstruction;

    public StateBasicConstructionAttack(BasicConstruction basicConstruction)
    {
        loadBasicConstruction = basicConstruction;
    }

    public void CheckSwitchStates()
    {
    }

    public void OnAwakeMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public void OnProcessingMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        loadBasicConstruction.BattleComponent.UseSkill(SkillUseConditionType.Contact);
        loadBasicConstruction.RevertToPreviousState();
    }

    public void OperateExit()
    {
    }

    public void OperateUpdate()
    {
    }
}