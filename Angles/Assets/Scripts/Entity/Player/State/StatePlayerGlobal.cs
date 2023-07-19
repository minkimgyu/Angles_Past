using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerGlobal : IState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerGlobal(Player player)
    {
        m_loadPlayer = player;
    }

    public void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnProcessingMessage(Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {

    }

    public void OperateExit()
    {

    }

    public void OperateUpdate()
    {
        if(m_loadPlayer.Data.RestoreDashRatio())
            m_loadPlayer.NotifyObservers(Player.ObserverType.ShowDashUI, m_loadPlayer.Data);
    }
}
