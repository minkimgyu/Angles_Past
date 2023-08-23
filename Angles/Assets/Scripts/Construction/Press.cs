using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : BasicConstruction
{
    [SerializeField]
    Transform endTr;
    public Transform EndTr { get { return endTr; } }


    Vector3 startPos;
    public Vector3 StartPos { get { return startPos; } }

    Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }

    public override void Init()
    {
        startPos = transform.position;

        rigid = GetComponent<Rigidbody2D>();

        BaseState<State> push = new StatePressPush(this);
        BaseState<State> pull = new StatePressPull(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Push, push);
        m_dicState.Add(State.Pull, pull);

        SetUp(State.Push);
    }
}

public class StatePressPush : BaseState<BasicConstruction.State>
{
    Press loadPress;

    // This at the top of your script
    public float speedOfChange = 1f;
    public float exponentialModifier = 10f;
    public float timeWeStarted;

    public StatePressPush(Press press)
    {
        loadPress = press;
    }

    public override void OnMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        timeWeStarted = Time.time;
    }

    public override void OperateExit()
    {
    }

    public override void ReceiveCollisionEnter(Collision2D collision) 
    {
        loadPress.SkillController.UseSkill(BaseSkill.UseConditionType.Contact);
    }

    public override void OperateUpdate()
    {
        // This is somehwere in the update where you are trying to ExponentialLerp
        float elapsedTime = Time.time - timeWeStarted;
        float stepAmount = Mathf.Pow (elapsedTime * speedOfChange, exponentialModifier);

        float ratio = Mathf.MoveTowards(0f, 1f, stepAmount);

        // FOR VECTOR3s
        //loadPress.transform.localPosition = Vector3.Lerp (Vector3.zero, loadPress.EndTr.localPosition, ratio);


        loadPress.Rigid.MovePosition(Vector3.Lerp(loadPress.transform.position, loadPress.EndTr.position, ratio));


        // 밀어내는 함수

        if (ratio >= 0.99)
        {
            loadPress.SetState(BasicConstruction.State.Pull);
        }
    }
}

public class StatePressPull : BaseState<BasicConstruction.State>
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

    public override void OnMessage(Telegram<BasicConstruction.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        SoundManager.Instance.PlaySFX(loadPress.transform.position, "Push", 0.5f);
        timeWeStarted = Time.time;
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
        // This is somehwere in the update where you are trying to ExponentialLerp
        float elapsedTime = Time.time - timeWeStarted;
        float stepAmount = Mathf.Pow(elapsedTime * speedOfChange, exponentialModifier);

        float ratio = Mathf.MoveTowards(0f, 1f, stepAmount);

        // FOR VECTOR3s
        //loadPress.transform.localPosition = Vector3.Lerp(loadPress.EndTr.localPosition, Vector3.zero, ratio);

        loadPress.Rigid.MovePosition(Vector3.Lerp(loadPress.transform.position, loadPress.StartPos, ratio));


        if (ratio >= 0.99)
        {
            loadPress.SetState(BasicConstruction.State.Push);
        }
    }
}
