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
        m_loadPlayer.UnderAttackAction += GoToGetDamageState;
    }

    void GoToGetDamageState(float damage, Vector2 dir, float thrust)
    {
        // 아래와 같은 state일 때만 작동
        if(m_loadPlayer.CurrentStateName == Player.State.Move || m_loadPlayer.CurrentStateName == Player.State.AttackReady)
        {
            Message<Player.State> message = new Message<Player.State>();
            message.dir = dir;
            message.damage = damage;
            message.thrust = thrust;

            Telegram<Player.State> telegram = new Telegram<Player.State>(Player.State.Damaged, message);
            m_loadPlayer.SetState(Player.State.Damaged, telegram);
        }
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
