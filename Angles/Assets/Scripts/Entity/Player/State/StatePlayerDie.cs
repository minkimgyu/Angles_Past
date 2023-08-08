using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDie : BaseState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerDie(Player player)
    {
        m_loadPlayer = player;
    }

    public override void CheckSwitchStates()
    {
    }

    public override void OnMessage(Telegram<Player.State> telegram)
    {
    }

    public void OnProcessingMessage(Telegram<Player.State> telegram)
    {
    }

    public override void OperateEnter()
    {
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}