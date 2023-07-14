using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerMove : IState<Player, Player.State>
{
    Player m_loadPlayer;

    public StatePlayerMove(Player player)
    {
        m_loadPlayer = player;
    }

    public void CheckSwitchStates(Player value)
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(Player value, Telegram<Player.State> message)
    {
        throw new System.NotImplementedException();
    }

    public void OnProcessingMessage(Player player, Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OperateEnter(Player player)
    {
        player.ActionJoystick.DashAction += UseDash;
        player.ActionJoystick.AttackReadyAction += GoToAttackReady;
    }

    void GoToAttackReady()
    {
        if (!m_loadPlayer.CheckOverMinValue(m_loadPlayer.ActionVec)) return;

        // Ready 전환
        m_loadPlayer.SetState(Player.State.AttackReady);
    }

    void UseDash() //  move joy and data
    {
        if (m_loadPlayer.Data.CanUseDash() == false || m_loadPlayer.MoveJoystick.ReturnMoveVec() == Vector2.zero) return;

        // 여기서 Dash 전환
        m_loadPlayer.SetState(Player.State.Dash);
    }

    public void OperateExit(Player player)
    {
        player.ActionJoystick.DashAction -= UseDash;
        player.ActionJoystick.AttackReadyAction -= GoToAttackReady;
    }

    public void OperateUpdate(Player player)
    {
        player.MoveComponent.Move(player.MoveVec, player.Data.Speed * player.Data.SpeedRatio, true);
    }
}
