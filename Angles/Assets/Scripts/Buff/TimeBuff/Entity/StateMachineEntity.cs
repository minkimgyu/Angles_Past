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

    protected Dictionary<T, BaseState<T>> m_dicState = new Dictionary<T, BaseState<T>>();

    //���� ���¸� ��� ������Ƽ.
    public BaseState<T> CurrentState { get; private set; }
    public BaseState<T> GlobalState { get; private set; }
    public BaseState<T> PreviousState { get; private set; }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (GlobalState != null)
            GlobalState.ReceiveTriggerEnter(collider); // global state�� �־��ֱ�

        if (m_dicState.ContainsKey(CurrentStateName) == false) return;
        m_dicState[CurrentStateName].ReceiveTriggerEnter(collider);
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (GlobalState != null)
            GlobalState.ReceiveTriggerExit(collider); // global state�� �־��ֱ�

        if (m_dicState.ContainsKey(CurrentStateName) == false) return;
        m_dicState[CurrentStateName].ReceiveTriggerExit(collider);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (GlobalState != null)
            GlobalState.ReceiveCollisionEnter(collision); // global state�� �־��ֱ�

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

        CurrentState.OnMessage(telegram);
        return true;
    }
    public void SetGlobalState(T stateName, BaseState<T> state) // �߻� �Լ� ó��
    {
        GlobalStateName = stateName; // �۷ι� ������Ʈ �̸� �ֱ�
        GlobalState = state;
        GlobalState.OperateEnter(); // �̷� ������ ����
    }

    //�������Ӹ��� ȣ��Ǵ� �Լ�.
    public void DoOperateUpdate()
    {
        if (GlobalState != null) GlobalState.OperateUpdate();

        if (CurrentState == null) return;

        CurrentState.OperateUpdate();
        CurrentState.CheckSwitchStates(); // update���� ������Ʈ ���� Ȯ�����ֱ�
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

    //�ܺο��� ������¸� �ٲ��ִ� �κ�.
    public bool SetState(T state)//IState<T, W> state) // �߻� �Լ� ó��
    {
        if (m_dicState.ContainsKey(state) == false) return false;


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
