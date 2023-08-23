using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDamaged : BaseState<Player.State>
{
    Player loadPlayer;

    float storedDamage;
    float storedThrust;
    Vector2 storedDir;

    public StatePlayerDamaged(Player player)
    {
        loadPlayer = player;
    }

    public override void CheckSwitchStates()
    {
        if (loadPlayer.DashComponent.NowFinish == true)
        {
            loadPlayer.RemoveEnemyContactKnockback();
            loadPlayer.RevertToPreviousState();
        }
    }

    public override void OnMessage(Telegram<Player.State> telegram)
    {
        storedDamage = telegram.Message.damage;
        storedThrust = telegram.Message.thrust;
        storedDir = telegram.Message.dir;
    }

    public override void OperateEnter()
    {
        Knockback(storedDir, storedThrust);
    }

    void Knockback(Vector2 dir, float thrust)
    {
        loadPlayer.DashComponent.CancelDash();
        loadPlayer.DashComponent.PlayDash(dir, thrust / loadPlayer.Weight.IntervalValue, loadPlayer.StunTime.IntervalValue);

        loadPlayer.AddEnemyContactKnockback();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}
