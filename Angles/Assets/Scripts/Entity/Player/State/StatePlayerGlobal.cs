using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerGlobal : IState<Player, Player.State>
{
    public void CheckSwitchStates(Player value)
    {
        throw new System.NotImplementedException();
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

    }

    public void OperateExit(Player player)
    {

    }

    public void OperateUpdate(Player player)
    {
        if(player.Data.RestoreDashRatio())
            player.NotifyObservers(Player.ObserverType.ShowDashUI, player.Data);
    }
}
