using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttackReady : IState<Player, Player.State>
{
    Player m_loadPlayer;

    public StatePlayerAttackReady(Player player)
    {
        m_loadPlayer = player;
    }

    public void CheckSwitchStates(Player value)
    {
        throw new System.NotImplementedException();
    }

    // �ٸ� state�ּ� ��ȯ�Ǿ� ���� ��� �۵��ϴ� �Լ�
    public void OnAwakeMessage(Player player, Telegram<Player.State> telegram)
    {
        
    }

    // �� ������Ʈ�� �޽����� �����ϴ� ��� �۵��ϴ� �Լ�
    public void OnProcessingMessage(Player player, Telegram<Player.State> telegram)
    {
        // AttackReady --> Attack
        if (telegram.SenderStateName == Player.State.AttackReady && telegram.Message.nextState == Player.State.Attack)
        {
            Message<Player.State> newMessage = new Message<Player.State>();
            newMessage.dir = telegram.Message.dir;
            newMessage.nextState = Player.State.Attack;
            Telegram<Player.State> newTelegram = new Telegram<Player.State>(Player.State.AttackReady, Player.State.Attack, newMessage);

            player.SetState(Player.State.Attack, newTelegram);
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

    public void OperateEnter(Player player)
    {
        player.ActionJoystick.AttackAction += Attack;
        player.ActionJoystick.AttackReadyAction += GoToMove;

        player.Animator.SetBool("NowReady", true);
        player.Data.ResetRushRatioToZero();
    }

    void GoToMove()
    {
        if (m_loadPlayer.CheckOverMinValue(m_loadPlayer.ActionVec)) return;

        // Ready ��ȯ
        m_loadPlayer.SetState(Player.State.Move);
    }

    public void OperateExit(Player player)
    {
        player.ActionJoystick.AttackAction -= Attack;
        player.ActionJoystick.AttackReadyAction -= GoToMove;

        player.Animator.SetBool("NowReady", false);
        player.NotifyObservers(Player.ObserverType.HideRushUI, player.Data);
    }

    public void OperateUpdate(Player player)
    {
        player.MoveComponent.Move(player.MoveVec, player.ActionVec, player.Data.ReadySpeed * player.Data.SpeedRatio, true);

        player.Data.RestoreRushRatio();
        player.NotifyObservers(Player.ObserverType.ShowRushUI, player.Data);
    }
}