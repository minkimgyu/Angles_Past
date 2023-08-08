using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDash : BaseState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerDash(Player player)
    {
        m_loadPlayer = player;
    }

    public override void CheckSwitchStates()
    {
        if (m_loadPlayer.DashComponent.NowFinish == true) m_loadPlayer.SetState(Player.State.Move);
    }

    public override void OnMessage(Telegram<Player.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        SoundManager.Instance.PlaySFX(m_loadPlayer.transform.position, "Dash", 1.5f);
        m_loadPlayer.DashComponent.PlayDash(m_loadPlayer.MoveVec, m_loadPlayer.PlayerData.DashThrust, m_loadPlayer.PlayerData.DashDuration);
        m_loadPlayer.PlayerData.SubtractDashRatio();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}