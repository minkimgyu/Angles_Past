using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGear : BasicConstruction
{
    [SerializeField]
    Transform[] movePoints;
    public Transform[] MovePoints { get { return movePoints; } }

    public override void Init()
    {
        BaseState<State> idle = new StateBasicConstructionIdle(this);
        BaseState<State> attack = new StateBasicConstructionAttack(this);
        BaseState<State> global = new StateMovingGearGlobal(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Idle, idle);
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.Global, global);

        SetUp(State.Idle);
        SetGlobalState(State.Global, global);
    }
}

public class StateMovingGearGlobal : BaseState<BasicConstruction.State>
{
    [SerializeField]
    float speed = 80;

    [SerializeField]
    float duration = 3;

    float storedTime = 0;

    float storedRotation = 0;
    MovingGear loadMovingGear;

    int nowIndex = 0;

    public StateMovingGearGlobal(MovingGear gear)
    {
        loadMovingGear = gear;
    }

    public override void OnMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        loadMovingGear.transform.localPosition = loadMovingGear.MovePoints[0].localPosition;
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
        storedRotation += Time.deltaTime * speed;
        loadMovingGear.transform.rotation = Quaternion.Euler(0, 0, storedRotation);

        MoveToNextPoint();
    }

    int NextIndex()
    {
        if(nowIndex + 1 > loadMovingGear.MovePoints.Length - 1)
        {
            return 0;
        }
        else
        {
            return nowIndex + 1;
        }
    }

    void MoveToNextPoint()
    {
        storedTime += Time.deltaTime;

        float ratio = storedTime / duration;

        int nextIndex = NextIndex();

        loadMovingGear.transform.localPosition = 
            Vector3.Lerp(loadMovingGear.MovePoints[nowIndex].localPosition, loadMovingGear.MovePoints[nextIndex].localPosition, ratio);
        // �о�� �Լ�

        if (ratio >= 0.99)
        {
            storedTime = 0;
            nowIndex = NextIndex();
        }
    }
}