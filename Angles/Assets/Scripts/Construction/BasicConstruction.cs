using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BasicConstruction : StateMachineEntity<BasicConstruction.State>
{
    //public Action ContactAction;

    public enum State
    {
        Global,
        Idle,
        Attack,

        Pull,
        Push
    }

    SkillController m_skillController;
    public SkillController SkillController { get { return m_skillController; } }

    ContactComponent m_contactComponent;
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    [SerializeField]
    protected GrantedSkill grantedUtilization;

    protected virtual void Awake()
    {
        m_skillController = GetComponent<SkillController>();
        m_contactComponent = GetComponent<ContactComponent>();
        Init();
    }

    private void Start()
    {
        grantedUtilization.LootSkillFromDB(m_skillController);
    }

    public abstract void Init();

    private void Update()
    {
        DoOperateUpdate();
    }

    protected override void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        ContactComponent.CallWhenCollisionEnter(col);
        base.OnCollisionEnter2D(col);
    }

    protected override void OnCollisionExit2D(Collision2D col)
    {
        ContactComponent.CallWhenCollisionExit(col);
        base.OnCollisionExit2D(col);
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
        loadBasicConstruction.SkillController.UseSkill(BaseSkill.UseConditionType.Contact);
        loadBasicConstruction.RevertToPreviousState();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}