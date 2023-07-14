using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message<T>
{
    public T nextState; // state 값을 string으로 변환해서 비교해주기
    public Vector2 dir;
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

public class StateMachineEntity<T, W> : Entity // T는 Entity, W는 State
{
    private T ownerEntity;

    public W CurrentStateName { get; private set; }
    public W GlobalStateName { get; private set; }
    public W PreviousStateName { get; private set; }

    protected Dictionary<W, IState<T, W>> m_dicState = new Dictionary<W, IState<T, W>>();

    //현재 상태를 담는 프로퍼티.
    public IState<T, W> CurrentState { get; private set; }
    public IState<T, W> GlobalState { get; private set; }
    public IState<T, W> PreviousState { get; private set; }

    //기본 상태를 생성시에 설정하게 생성자 만들기.
    protected void SetUp(T owner, W defaultState) // 가상 함수 처리
    {
        ownerEntity = owner;
        CurrentState = null;
        GlobalState = null;
        PreviousState = null;

        SetState(defaultState);
    }

    public bool SwitchStateAndSendMessage(W state, Telegram<W> telegram)
    {



        return SetState(state, telegram);
    }

    public bool CanSendMessage(Telegram<W> telegram)
    {
        if (CurrentState == null || telegram == null || !CurrentStateName.Equals(telegram.ReceiverStateName)) return false;

        return true;
    }
    
    public bool HandleInitMessage(Telegram<W> telegram)
    {
        if (!CanSendMessage(telegram)) return false;

        CurrentState.OnAwakeMessage(ownerEntity, telegram);
        return true;
    }

    public bool HandleProcessingMessage(Telegram<W> telegram)
    {
        if (!CanSendMessage(telegram)) return false;

        CurrentState.OnProcessingMessage(ownerEntity, telegram);
        return true;
    }

    public void SetGlobalState(IState<T, W> state) // 추상 함수 처리
    {
        GlobalState = state;
    }

    //매프레임마다 호출되는 함수.
    public void DoOperateUpdate()
    {
        if (GlobalState != null) GlobalState.OperateUpdate(ownerEntity);

        if (CurrentState != null) CurrentState.OperateUpdate(ownerEntity);
    }


    public bool RevertToPreviousState()
    {
        return SetState(PreviousStateName);
    }

    public bool RevertToPreviousState(Telegram<W> message)
    {
        return SetState(PreviousStateName, message);
    }

    //외부에서 현재상태를 바꿔주는 부분.
    public bool SetState(W state)//IState<T, W> state) // 추상 함수 처리
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
            CurrentState.OperateExit(ownerEntity);

        //상태 교체.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (CurrentState != null) //새 상태의 Enter를 호출한다.
            CurrentState.OperateEnter(ownerEntity);

        return true;
    }

    //외부에서 현재상태를 바꿔주는 부분.
    public bool SetState(W state, Telegram<W> message) // 추상 함수 처리
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
            CurrentState.OperateExit(ownerEntity);

        //상태 교체.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (HandleInitMessage(message) == false)
        {
            Debug.Log("메시지 전달에 실패했습니다");
            return false;
        }

        if (CurrentState != null) //새 상태의 Enter를 호출한다.
            CurrentState.OperateEnter(ownerEntity);

        return true;
    }
}
