using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : BasicConstruction
{
    public override void Init()
    {
        BaseState<State> idle = new StateBasicConstructionIdle(this);
        BaseState<State> attack = new StateBasicConstructionAttack(this);
        BaseState<State> global = new StateGearGlobal(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Idle, idle);
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.Global, global);

        SetUp(State.Idle);
        SetGlobalState(global);
    }
}

public class StateGearGlobal : BaseState<BasicConstruction.State>
{
    [SerializeField]
    float speed = 80;

    float storedRotation = 0;
    Gear loadGear;

    public StateGearGlobal(Gear gear)
    {
        loadGear = gear;
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

    public override void OperateUpdate()
    {
        storedRotation += Time.deltaTime * speed;
        loadGear.transform.rotation = Quaternion.Euler(0, 0, storedRotation);
    }
}