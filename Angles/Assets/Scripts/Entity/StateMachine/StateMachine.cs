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

    ////���� ���¸� ��� ������Ƽ.
    //public IState<T, W> CurrentState { get; private set; }
    //public IState<T, W> GlobalState { get; private set; }
    //public IState<T, W> PreviousState { get; private set; }

    ////�⺻ ���¸� �����ÿ� �����ϰ� ������ �����.
    //public StateMachine(T owner, IState<T, W> defaultState) // ���� �Լ� ó��
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

    //public void SetGlobalState(IState<T, W> state) // �߻� �Լ� ó��
    //{
    //    GlobalState = state;
    //}

    ////�������Ӹ��� ȣ��Ǵ� �Լ�.
    //public void DoOperateUpdate()
    //{
    //    if (GlobalState != null) GlobalState.OperateUpdate(ownerEntity);

    //    if (CurrentState != null) CurrentState.OperateUpdate(ownerEntity);
    //}


    ////�ܺο��� ������¸� �ٲ��ִ� �κ�.
    //public void SetState(IState<T, W> state) // �߻� �Լ� ó��
    //{
    //    //���� �ൿ�� ���̾ �������� ���ϵ��� ����ó��.
    //    //���� ���, ���� �������ε� �� ������ �ϴ� �������� ���׸� �����Ҽ��� �ִ�.
    //    if (CurrentState == state)
    //    {
    //        //Debug.Log("���� �̹� �ش� �����Դϴ�.");
    //        return;
    //    }

    //    PreviousState = CurrentState;

    //    if (CurrentState != null) //���°� �ٲ�� ����, ���� ������ Exit�� ȣ���Ѵ�.
    //        CurrentState.OperateExit(ownerEntity);

    //    //���� ��ü.
    //    CurrentState = state;

    //    if (CurrentState != null) //�� ������ Enter�� ȣ���Ѵ�.
    //        CurrentState.OperateEnter(ownerEntity);
    //}

    ////�ܺο��� ������¸� �ٲ��ִ� �κ�.
    //public bool SetState(IState<T, W> state, W message) // �߻� �Լ� ó��
    //{
    //    //���� �ൿ�� ���̾ �������� ���ϵ��� ����ó��.
    //    //���� ���, ���� �������ε� �� ������ �ϴ� �������� ���׸� �����Ҽ��� �ִ�.
    //    if (CurrentState == state)
    //    {
    //        //Debug.Log("���� �̹� �ش� �����Դϴ�.");
    //        return false;
    //    }

    //    PreviousState = CurrentState;

    //    if (CurrentState != null) //���°� �ٲ�� ����, ���� ������ Exit�� ȣ���Ѵ�.
    //        CurrentState.OperateExit(ownerEntity);

         

    //    //���� ��ü.
    //    CurrentState = state;

    //    if (HandleMessage(message) == false)
    //    {
    //        Debug.Log("�޽��� ���޿� �����߽��ϴ�");
    //        return false;
    //    }

    //    if (CurrentState != null) //�� ������ Enter�� ȣ���Ѵ�.
    //        CurrentState.OperateEnter(ownerEntity);

    //    return true;
    //}
//}