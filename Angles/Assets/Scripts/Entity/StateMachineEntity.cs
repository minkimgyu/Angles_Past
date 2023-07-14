using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message<T>
{
    public T nextState; // state ���� string���� ��ȯ�ؼ� �����ֱ�
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
    /// �ڽ� Ŭ������ �޽����� ������ ���
    /// </summary>
    public Telegram(T receiver, Message<T> message) 
    {
        _senderStateName = receiver;
        _receiverStatename = receiver;
        _message = message;
    }
}

public class StateMachineEntity<T, W> : Entity // T�� Entity, W�� State
{
    private T ownerEntity;

    public W CurrentStateName { get; private set; }
    public W GlobalStateName { get; private set; }
    public W PreviousStateName { get; private set; }

    protected Dictionary<W, IState<T, W>> m_dicState = new Dictionary<W, IState<T, W>>();

    //���� ���¸� ��� ������Ƽ.
    public IState<T, W> CurrentState { get; private set; }
    public IState<T, W> GlobalState { get; private set; }
    public IState<T, W> PreviousState { get; private set; }

    //�⺻ ���¸� �����ÿ� �����ϰ� ������ �����.
    protected void SetUp(T owner, W defaultState) // ���� �Լ� ó��
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

    public void SetGlobalState(IState<T, W> state) // �߻� �Լ� ó��
    {
        GlobalState = state;
    }

    //�������Ӹ��� ȣ��Ǵ� �Լ�.
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

    //�ܺο��� ������¸� �ٲ��ִ� �κ�.
    public bool SetState(W state)//IState<T, W> state) // �߻� �Լ� ó��
    {
        //���� �ൿ�� ���̾ �������� ���ϵ��� ����ó��.
        //���� ���, ���� �������ε� �� ������ �ϴ� �������� ���׸� �����Ҽ��� �ִ�.

        if (CurrentState == m_dicState[state])
        {
            //Debug.Log("���� �̹� �ش� �����Դϴ�.");
            return false;
        }

        PreviousState = CurrentState;
        PreviousStateName = CurrentStateName;

        if (CurrentState != null) //���°� �ٲ�� ����, ���� ������ Exit�� ȣ���Ѵ�.
            CurrentState.OperateExit(ownerEntity);

        //���� ��ü.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (CurrentState != null) //�� ������ Enter�� ȣ���Ѵ�.
            CurrentState.OperateEnter(ownerEntity);

        return true;
    }

    //�ܺο��� ������¸� �ٲ��ִ� �κ�.
    public bool SetState(W state, Telegram<W> message) // �߻� �Լ� ó��
    {
        //���� �ൿ�� ���̾ �������� ���ϵ��� ����ó��.
        //���� ���, ���� �������ε� �� ������ �ϴ� �������� ���׸� �����Ҽ��� �ִ�.
        if (CurrentState == m_dicState[state])
        {
            //Debug.Log("���� �̹� �ش� �����Դϴ�.");
            return false;
        }

        PreviousState = CurrentState;
        PreviousStateName = CurrentStateName;

        if (CurrentState != null) //���°� �ٲ�� ����, ���� ������ Exit�� ȣ���Ѵ�.
            CurrentState.OperateExit(ownerEntity);

        //���� ��ü.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (HandleInitMessage(message) == false)
        {
            Debug.Log("�޽��� ���޿� �����߽��ϴ�");
            return false;
        }

        if (CurrentState != null) //�� ������ Enter�� ȣ���Ѵ�.
            CurrentState.OperateEnter(ownerEntity);

        return true;
    }
}
