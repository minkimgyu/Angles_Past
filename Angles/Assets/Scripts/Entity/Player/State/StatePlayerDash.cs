using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDash : IState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerDash(Player player)
    {
        m_loadPlayer = player;
    }

    public void CheckSwitchStates()
    {
        if (m_loadPlayer.DashComponent.NowFinish == true) m_loadPlayer.SetState(Player.State.Move);
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
        m_loadPlayer.DashComponent.PlayDash(m_loadPlayer.MoveVec, m_loadPlayer.Data.DashThrust, m_loadPlayer.Data.DashTime);
        m_loadPlayer.Data.SubtractDashRatio();
    }

    public void OperateExit()
    {
        
    }

    public void OperateUpdate()
    {
        CheckSwitchStates();
    }
}