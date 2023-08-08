using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerGlobal : BaseState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerGlobal(Player player)
    {
        m_loadPlayer = player;
    }

    public override void CheckSwitchStates()
    {
    }

    public override void OnMessage(Telegram<Player.State> telegram)
    {
    }

    public override void ReceiveUnderAttack(float damage, Vector2 dir, float thrust) 
    {
        GoToGetDamageState(damage, dir, thrust);
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

    public override void ReceiveTriggerEnter(Collider2D collider) 
    {
        m_loadPlayer.LootingItemComponent.LootingItem(collider, m_loadPlayer.BattleComponent);
    }

    public override void ReceiveCollisionEnter(Collision2D collision) 
    {
        m_loadPlayer.ContactComponent.CallWhenCollisionEnter(collision);
    }

    public override void ReceiveCollisionExit(Collision2D collision) 
    {
        m_loadPlayer.ContactComponent.CallWhenCollisionExit(collision);
    }

    public override void OperateEnter()
    {
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
        if(m_loadPlayer.PlayerData.RestoreDashRatio())
            m_loadPlayer.NotifyObservers(Player.ObserverType.ShowDashUI, m_loadPlayer.PlayerData);
    }
}
