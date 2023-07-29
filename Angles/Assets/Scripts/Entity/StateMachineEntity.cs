using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message<T>
{
    public T nextState; // state ���� string���� ��ȯ�ؼ� �����ֱ�
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
    /// �ڽ� Ŭ������ �޽����� ������ ���
    /// </summary>
    public Telegram(T receiver, Message<T> message) 
    {
        _senderStateName = receiver;
        _receiverStatename = receiver;
        _message = message;
    }
}

abstract public class StateMachineEntity<T> : Entity // T�� Entity, W�� State
{
    public T CurrentStateName { get; private set; }
    public T GlobalStateName { get; private set; }
    public T PreviousStateName { get; private set; }

    protected Dictionary<T, IState<T>> m_dicState = new Dictionary<T, IState<T>>();

    //���� ���¸� ��� ������Ƽ.
    public IState<T> CurrentState { get; private set; }
    public IState<T> GlobalState { get; private set; }
    public IState<T> PreviousState { get; private set; }

    

    //�⺻ ���¸� �����ÿ� �����ϰ� ������ �����.
    protected void SetUp(T defaultState) // ���� �Լ� ó��
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

        CurrentState.OnAwakeMessage(telegram);
        return true;
    }

    public bool HandleProcessingMessage(Telegram<T> telegram)
    {
        if (!CanSendMessage(telegram)) return false;

        CurrentState.OnProcessingMessage(telegram);
        return true;
    }

    public void SetGlobalState(IState<T> state) // �߻� �Լ� ó��
    {
        GlobalState = state;
        state.OnSetToGlobalState();
    }

    //�������Ӹ��� ȣ��Ǵ� �Լ�.
    public void DoOperateUpdate()
    {
        if (GlobalState != null) GlobalState.OperateUpdate();

        if (CurrentState != null) CurrentState.OperateUpdate();
    }


    public bool RevertToPreviousState()
    {
        return SetState(PreviousStateName);
    }

    public bool RevertToPreviousState(Telegram<T> message)
    {
        return SetState(PreviousStateName, message);
    }

    //�ܺο��� ������¸� �ٲ��ִ� �κ�.
    public bool SetState(T state)//IState<T, W> state) // �߻� �Լ� ó��
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
            CurrentState.OperateExit();

        //���� ��ü.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (CurrentState != null) //�� ������ Enter�� ȣ���Ѵ�.
            CurrentState.OperateEnter();

        return true;
    }

    //�ܺο��� ������¸� �ٲ��ִ� �κ�.
    public bool SetState(T state, Telegram<T> message) // �߻� �Լ� ó��
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
            CurrentState.OperateExit();

        //���� ��ü.
        CurrentState = m_dicState[state];
        CurrentStateName = state;

        if (HandleInitMessage(message) == false)
        {
            Debug.Log("�޽��� ���޿� �����߽��ϴ�");
            return false;
        }

        if (CurrentState != null) //�� ������ Enter�� ȣ���Ѵ�.
            CurrentState.OperateEnter();

        return true;
    }
}
