using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message<T>
{
    public T nextState; // state 값을 string으로 변환해서 비교해주기
    public Vector2 dir;
    public float damage;
    public float thrust;
    public Entity contactedEntity;
}

public class Telegram<T>
{
    T _senderStateName;
    T _receiverStatename;

    public T SenderStateName
    {
        get { return _senderStateName; }
    }

    public T ReceiverStateName
    {
        get { return _receiverStatename; }
    }

    Message<T> _message;
    public Message<T> Message
    {
        get { return _message; }
    }


    public bool IsSameState(T nowState)
    {
        return nowState.Equals(_senderStateName) || _senderStateName.Equals(_receiverStatename);
    }

    public bool CheckState(T start, T end)
    {
        return start.Equals(_senderStateName) && end.Equals(_receiverStatename);
    }

    public Telegram(T sender, T receiver, Message<T> message)
    {
        _senderStateName = sender;
        _receiverStatename = receiver;
        _message = message;
    }

    /// <summary>
    /// 자식 클레스가 메시지를 보내는 경우
    /// </summary>
    public Telegram(T receiver, Message<T> message) 
    {
        _senderStateName = receiver;
        _receiverStatename = receiver;
        _message = message;
    }
}

abstract public class StateMachineEntity<T> : Entity // T는 Entity, W는 State
{
    public T CurrentStateName { get; private set; }

    public T GlobalStateName { get; private set; }

    public T PreviousStateName { get; private set; }

    protected Dictionary<T, BaseState<T>> m_dicState = new Dictionary<T, BaseState<T>>();

    //현재 상태를 담는 프로퍼티.
    public BaseState<T> CurrentState { get; private set; }
    public BaseState<T> GlobalState { get; private set; }
    public BaseState<T> PreviousState { get; private set; }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (GlobalState != null)
            GlobalState.ReceiveTriggerEnter(collider); // global state도 넣어주기

        if (m_dicState.ContainsKey(CurrentStateName) == false) return;
        m_dicState[CurrentStateName].ReceiveTriggerEnter(collider);
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (GlobalState != null)
            GlobalState.ReceiveTriggerExit(collider); // global state도 넣어주기

        if (m_dicState.ContainsKey(CurrentStateName) == false) return;
        m_dicState[CurrentStateName].ReceiveTriggerExit(collider);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (GlobalState != null)
            GlobalState.ReceiveCollisionEnter(collision); // global state도 넣어주기

        if (m_dicState.ContainsKey(CurrentStateName))
            m_dicState[CurrentStateName].ReceiveCollisionEnter(collision);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (GlobalState != null)
            GlobalState.ReceiveCollisionExit(collision);

        if (m_dicState.ContainsKey(CurrentStateName))
            m_dicState[CurrentStateName].ReceiveCollisionExit(collision);
    }

    protected virtual void OnEnable()
    {
        if (GlobalState != null)
            GlobalState.ReceiveOnEnable();

        if (m_dicState.ContainsKey(CurrentStateName))
            m_dicState[CurrentStateName].ReceiveOnEnable();
    }

    protected virtual void OnDisable()
    {
        if (GlobalState != null)
            GlobalState.ReceiveOnDisable();

        if (m_dicState.ContainsKey(CurrentStateName))
            m_dicState[CurrentStateName].ReceiveOnDisable();
    }

    //기본 상태를 생성시에 설정하게 생성자 만들기.
    protected void SetUp(T defaultState) // 가상 함수 처리
    {
        CurrentState = null;
        GlobalState = null;
        PreviousState = null;

        SetState(defaultState);
    }

    public bool SwitchStateAndSendMessage(T state, Telegram<T> telegram)
    {
        return SetState(state, telegram);
    }

    public bool CanSendMessage(Telegram<T> telegram)
    {
        if (CurrentState == null || telegram == null || !CurrentStateName.Equals(telegram.ReceiverStateName)) return false;

        return true;
    }
    
    public bool HandleInitMessage(Telegram<T> telegram)
    {
        if (!CanSendMessage(telegram)) return false;

        CurrentState.OnMessage(telegram);
        return true;
    }
    public void SetGlobalState(T stateName, BaseState<T> state) // 추상 함수 처리
    {
        GlobalStateName = stateName; // 글로벌 스테이트 이름 넣기
        GlobalState = state;
        GlobalState.OperateEnter(); // 이런 식으로 동작
    }

    //매프레임마다 호출되는 함수.
    public void DoOperateUpdate()
    {
        if (GlobalState != null) GlobalState.OperateUpdate();

        if (CurrentState == null) return;

        CurrentState.OperateUpdate();
        CurrentState.CheckSwitchStates(); // update에서 스테이트 변경 확인해주기
    }

    private void Update()
    {
        DoOperateUpdate();
    }

    public bool RevertToPreviousState()
    {
        return SetState(PreviousStateName);
    }

    public bool RevertToPreviousState(Telegram<T> message)
    {
        return SetState(PreviousStateName, message);
    }

    //외부에서 현재상태를 바꿔주는 부분.
    public bool SetState(T state)//IState<T, W> state) // 추상 함수 처리
    {
        if (m_dicState.ContainsKey(state) == false) return false;


        //같은 행동을 연이어서 세팅하지 못하도록 예외처리.
        //예를 들어, 지금 점프중인데 또 점프를 하는 무한점프 버그를 예방할수도 있다.
        if (CurrentState == m_dicState[state])
        {
            //Debug.Log("현재 이미 해당 상태입니다.");
            return false;
        }

        PreviousState = CurrentState;
        PreviousStateName = CurrentStateName;

        if (CurrentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출한다.
            CurrentState.OperateExit();

        //상태 교체.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (CurrentState != null) //새 상태의 Enter를 호출한다.
            CurrentState.OperateEnter();

        return true;
    }

    //외부에서 현재상태를 바꿔주는 부분.
    public bool SetState(T state, Telegram<T> message) // 추상 함수 처리
    {
        //같은 행동을 연이어서 세팅하지 못하도록 예외처리.
        //예를 들어, 지금 점프중인데 또 점프를 하는 무한점프 버그를 예방할수도 있다.
        if (CurrentState == m_dicState[state])
        {
            //Debug.Log("현재 이미 해당 상태입니다.");
            return false;
        }

        PreviousState = CurrentState;
        PreviousStateName = CurrentStateName;

        if (CurrentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출한다.
            CurrentState.OperateExit();

        //상태 교체.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (HandleInitMessage(message) == false)
        {
            Debug.Log("메시지 전달에 실패했습니다");
            return false;
        }

        if (CurrentState != null) //새 상태의 Enter를 호출한다.
            CurrentState.OperateEnter();

        return true;
    }
}
