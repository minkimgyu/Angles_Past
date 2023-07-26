using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : BasicConstruction
{
    void Start()
    {
        Init();
    }    

    public override void Init()
    {
        IState<State> idle = new StateBasicConstructionIdle(this);
        IState<State> attack = new StateBasicConstructionAttack(this);
        IState<State> global = new StateGearGlobal(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Idle, idle);
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.Global, global);

        SetUp(State.Idle);
        SetGlobalState(global);
        grantedUtilization.LootSkillFromDB(BattleComponent);
    }
}

public class StateGearGlobal : IState<BasicConstruction.State>
{
    [SerializeField]
    float speed = 80;

    float storedRotation = 0;
    Gear loadGear;

    public StateGearGlobal(Gear gear)
    {
        loadGear = gear;
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
    }

    public void OperateExit()
    {
    }

    public virtual void OperateUpdate()
    {
        storedRotation += Time.deltaTime * speed;
        loadGear.transform.rotation = Quaternion.Euler(0, 0, storedRotation);
    }
}