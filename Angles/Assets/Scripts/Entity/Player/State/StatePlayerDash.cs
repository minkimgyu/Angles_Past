using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDash : IState<Player, Player.State>
{
    public void CheckSwitchStates(Player player)
    {
        if (player.DashComponent.NowFinish == true) player.SetState(Player.State.Move);
    }

    public void OnAwakeMessage(Player value, Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnProcessingMessage(Player value, Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OperateEnter(Player player)
    {
        player.DashComponent.PlayDash(player.MoveVec, player.Data.DashThrust, player.Data.DashTime);
        player.Data.SubtractDashRatio();
    }

    public void OperateExit(Player player)
    {
        
    }

    public void OperateUpdate(Player player)
    {
        CheckSwitchStates(player);
    }
}