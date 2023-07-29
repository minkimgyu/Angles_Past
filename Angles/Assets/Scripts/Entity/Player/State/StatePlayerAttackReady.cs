using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttackReady : IState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerAttackReady(Player player)
    {
        m_loadPlayer = player;
    }

    public void CheckSwitchStates()
    {
    }

    // �ٸ� state�ּ� ��ȯ�Ǿ� ���� ��� �۵��ϴ� �Լ�
    public void OnAwakeMessage(Telegram<Player.State> telegram)
    {
    }

    // �� ������Ʈ�� �޽����� �����ϴ� ��� �۵��ϴ� �Լ�
    public void OnProcessingMessage(Telegram<Player.State> telegram)
    {
        // AttackReady --> Attack
        if (telegram.SenderStateName == Player.State.AttackReady && telegram.Message.nextState == Player.State.Attack)
        {
            Message<Player.State> newMessage = new Message<Player.State>();
            newMessage.dir = telegram.Message.dir;
            newMessage.nextState = Player.State.Attack;
            Telegram<Player.State> newTelegram = new Telegram<Player.State>(Player.State.AttackReady, Player.State.Attack, newMessage);

            m_loadPlayer.SetState(Player.State.Attack, newTelegram);
        }
            
    }

    void Attack()
    {
        if (!m_loadPlayer.CheckOverMinValue(m_loadPlayer.ActionVec)) return;

        Message<Player.State> message = new Message<Player.State>();
        message.dir = -m_loadPlayer.ActionVec;
        Telegram<Player.State> telegram = new Telegram<Player.State>(Player.State.AttackReady, Player.State.Attack, message);

        m_loadPlayer.SetState(Player.State.Attack, telegram);
    }

    public void OperateEnter()
    {
        m_loadPlayer.ActionJoystick.AttackAction += Attack;
        m_loadPlayer.ActionJoystick.AttackReadyAction += GoToMove;

        m_loadPlayer.Animator.SetBool("NowReady", true);
        m_loadPlayer.Data.ResetRushRatioToZero();
    }

    void GoToMove()
    {
        if (m_loadPlayer.CheckOverMinValue(m_loadPlayer.ActionVec)) return;

        // Ready ��ȯ
        m_loadPlayer.SetState(Player.State.Move);
    }

    public void OperateExit()
    {
        m_loadPlayer.ActionJoystick.AttackAction -= Attack;
        m_loadPlayer.ActionJoystick.AttackReadyAction -= GoToMove;

        m_loadPlayer.Animator.SetBool("NowReady", false);
        m_loadPlayer.NotifyObservers(Player.ObserverType.HideRushUI, m_loadPlayer.Data);
    }

    public void OperateUpdate()
    {
        m_loadPlayer.MoveComponent.Move(m_loadPlayer.MoveVec, m_loadPlayer.ActionVec, m_loadPlayer.Data.ReadySpeed.IntervalValue * m_loadPlayer.Data.SpeedRatio, true);

        m_loadPlayer.MoveComponent.RotationPlayer(m_loadPlayer.ActionVec, true);

        m_loadPlayer.Data.RestoreRushRatio();
        m_loadPlayer.NotifyObservers(Player.ObserverType.ShowRushUI, m_loadPlayer.Data);
    }

    public void OnSetToGlobalState()
    {
    }
}