using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : BasicConstruction
{
    [SerializeField]
    Transform endTr;
    public Transform EndTr { get { return endTr; } }

    Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();

        IState<State> push = new StatePressPush(this);
        IState<State> pull = new StatePressPull(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Push, push);
        m_dicState.Add(State.Pull, pull);

        SetUp(State.Push);
        grantedUtilization.LootSkillFromDB(BattleComponent);
    }
}

public class StatePressPush : IState<BasicConstruction.State>
{
    Press loadPress;
    Vector3 nowPos;

    // This at the top of your script
    public float speedOfChange = 1f;
    public float exponentialModifier = 10f;
    public float timeWeStarted;

    public StatePressPush(Press press)
    {
        loadPress = press;
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
        loadPress.ContactAction += GoToAttackState;
        // This code should be maybe in a function that you call which starts the ExponentialLerp
        timeWeStarted = Time.time;
        nowPos = loadPress.transform.position;
    }

    public void OperateExit()
    {
        loadPress.ContactAction -= GoToAttackState;
    }

    void GoToAttackState()
    {
        loadPress.BattleComponent.UseSkill(SkillUseConditionType.Contact);
    }

    public void OperateUpdate()
    {
        // This is somehwere in the update where you are trying to ExponentialLerp
        float elapsedTime = Time.time - timeWeStarted;
        float stepAmount = Mathf.Pow (elapsedTime * speedOfChange, exponentialModifier);

        float ratio = Mathf.MoveTowards(0f, 1f, stepAmount);

        // FOR VECTOR3s
        loadPress.transform.localPosition = Vector3.Lerp (Vector3.zero, loadPress.EndTr.localPosition, ratio);
        // 밀어내는 함수

        if(ratio >= 0.99)
        {
            loadPress.SetState(BasicConstruction.State.Pull);
        }
    }
}

public class StatePressPull : IState<BasicConstruction.State>
{
    Press loadPress;

    // This at the top of your script
    public float speedOfChange = 1f;
    public float exponentialModifier = 10f;
    public float timeWeStarted;

    public StatePressPull(Press press)
    {
        loadPress = press;
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
        timeWeStarted = Time.time;
    }

    public void OperateExit()
    {
    }

    void GoToAttackState()
    {
    }

    public void OperateUpdate()
    {
        // This is somehwere in the update where you are trying to ExponentialLerp
        float elapsedTime = Time.time - timeWeStarted;
        float stepAmount = Mathf.Pow(elapsedTime * speedOfChange, exponentialModifier);

        float ratio = Mathf.MoveTowards(0f, 1f, stepAmount);

        // FOR VECTOR3s
        loadPress.transform.localPosition = Vector3.Lerp(loadPress.EndTr.localPosition, Vector3.zero, ratio);

        if (ratio >= 0.99)
        {
            loadPress.SetState(BasicConstruction.State.Push);
        }
    }
}
