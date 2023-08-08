using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct Message
//{
//    public Vector2 dir;
//}

//public class Telegram<T>
//{
//    T _senderState;
//    T _receiverState;

//    public T SenderState
//    {
//        get { return _senderState; }
//    }

//    public T ReceiverState
//    {
//        get { return _receiverState; }
//    }

//    Message _message;
//    public Message Message
//    {
//        get { return _message; }
//    }

//    public Telegram(T sender, T receiver, Message message)
//    {
//        _senderState = sender;
//        _receiverState = receiver;
//        _message = message;
//    }
//}

//public class StateMachine<T, W> where T : class
//{
    //private T ownerEntity;

    ////현재 상태를 담는 프로퍼티.
    //public IState<T, W> CurrentState { get; private set; }
    //public IState<T, W> GlobalState { get; private set; }
    //public IState<T, W> PreviousState { get; private set; }

    ////기본 상태를 생성시에 설정하게 생성자 만들기.
    //public StateMachine(T owner, IState<T, W> defaultState) // 가상 함수 처리
    //{
    //    ownerEntity = owner;
    //    CurrentState = null;
    //    GlobalState = null;
    //    PreviousState = null;

    //    SetState(defaultState);
    //}

    //public bool SwitchStateAndSendMessage(IState<T, W> state, W telegram)
    //{
    //    return SetState(state, telegram);
    //}

    //public bool HandleMessage(W telegram)
    //{
    //    if (CurrentState == null || telegram == null) return false;

    //    CurrentState.OnMessage(ownerEntity, telegram);
    //    return true;
    //}

    //public void RevertToPreviousState()
    //{
    //    SetState(PreviousState);
    //}

    //public void RevertToPreviousStateWithMessage(W message)
    //{
    //    SetState(PreviousState, message);
    //}

    //public void SetGlobalState(IState<T, W> state) // 추상 함수 처리
    //{
    //    GlobalState = state;
    //}

    ////매프레임마다 호출되는 함수.
    //public void DoOperateUpdate()
    //{
    //    if (GlobalState != null) GlobalState.OperateUpdate(ownerEntity);

    //    if (CurrentState != null) CurrentState.OperateUpdate(ownerEntity);
    //}


    ////외부에서 현재상태를 바꿔주는 부분.
    //public void SetState(IState<T, W> state) // 추상 함수 처리
    //{
    //    //같은 행동을 연이어서 세팅하지 못하도록 예외처리.
    //    //예를 들어, 지금 점프중인데 또 점프를 하는 무한점프 버그를 예방할수도 있다.
    //    if (CurrentState == state)
    //    {
    //        //Debug.Log("현재 이미 해당 상태입니다.");
    //        return;
    //    }

    //    PreviousState = CurrentState;

    //    if (CurrentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출한다.
    //        CurrentState.OperateExit(ownerEntity);

    //    //상태 교체.
    //    CurrentState = state;

    //    if (CurrentState != null) //새 상태의 Enter를 호출한다.
    //        CurrentState.OperateEnter(ownerEntity);
    //}

    ////외부에서 현재상태를 바꿔주는 부분.
    //public bool SetState(IState<T, W> state, W message) // 추상 함수 처리
    //{
    //    //같은 행동을 연이어서 세팅하지 못하도록 예외처리.
    //    //예를 들어, 지금 점프중인데 또 점프를 하는 무한점프 버그를 예방할수도 있다.
    //    if (CurrentState == state)
    //    {
    //        //Debug.Log("현재 이미 해당 상태입니다.");
    //        return false;
    //    }

    //    PreviousState = CurrentState;

    //    if (CurrentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출한다.
    //        CurrentState.OperateExit(ownerEntity);

         

    //    //상태 교체.
    //    CurrentState = state;

    //    if (HandleMessage(message) == false)
    //    {
    //        Debug.Log("메시지 전달에 실패했습니다");
    //        return false;
    //    }

    //    if (CurrentState != null) //새 상태의 Enter를 호출한다.
    //        CurrentState.OperateEnter(ownerEntity);

    //    return true;
    //}
//}