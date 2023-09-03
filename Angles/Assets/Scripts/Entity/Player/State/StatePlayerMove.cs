using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerMove : BaseState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerMove(Player player)
    {
        m_loadPlayer = player;
    }

    public override void OnMessage(Telegram<Player.State> message)
    {
    }

    public override void OperateEnter()
    {
        m_loadPlayer.ActionJoystick.DashAction += UseDash;
        m_loadPlayer.ActionJoystick.AttackReadyAction += GoToAttackReady;
    }

    void GoToAttackReady()
    {
        if (!m_loadPlayer.CheckOverMinValue(m_loadPlayer.ActionVec)) return;

        // Ready 전환
        m_loadPlayer.SetState(Player.State.AttackReady);
    }

    void UseDash() //  move joy and data
    {
        if (CanUseDash() == false || m_loadPlayer.MoveJoystick.ReturnMoveVec() == Vector2.zero) return;

        // 여기서 Dash 전환
        m_loadPlayer.SetState(Player.State.Dash);
    }

    public bool CanUseDash()
    {
        if (m_loadPlayer.DashRatio - 1 / m_loadPlayer.DashCount.IntervalValue >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OperateExit()
    {
        m_loadPlayer.ActionJoystick.DashAction -= UseDash;
        m_loadPlayer.ActionJoystick.AttackReadyAction -= GoToAttackReady;
    }

    public override void OperateUpdate()
    {
        m_loadPlayer.MoveComponent.Move(m_loadPlayer.MoveVec, m_loadPlayer.Speed.IntervalValue);
        m_loadPlayer.MoveComponent.RotationPlayer(m_loadPlayer.MoveVec.normalized, true);
    }
}
