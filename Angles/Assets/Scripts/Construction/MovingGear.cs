using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGear : BasicConstruction
{
    [SerializeField]
    Transform[] movePoints;
    public Transform[] MovePoints { get { return movePoints; } }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        IState<State> idle = new StateBasicConstructionIdle(this);
        IState<State> attack = new StateBasicConstructionAttack(this);
        IState<State> global = new StateMovingGearGlobal(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Idle, idle);
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.Global, global);

        SetUp(State.Idle);
        SetGlobalState(global);
        grantedUtilization.LootSkillFromDB(BattleComponent);
    }
}

public class StateMovingGearGlobal : IState<BasicConstruction.State>
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
        loadMovingGear.transform.localPosition = loadMovingGear.MovePoints[0].localPosition;
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