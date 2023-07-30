using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDamaged : IState<Player.State>
{
    Player loadPlayer;

    float storedDamage;
    float storedThrust;
    Vector2 storedDir;

    public StatePlayerDamaged(Player player)
    {
        loadPlayer = player;
    }

    public void CheckSwitchStates()
    {
        if (loadPlayer.DashComponent.NowFinish == true) loadPlayer.RevertToPreviousState();
    }

    public void OnAwakeMessage(Telegram<Player.State> telegram)
    {
        storedDamage = telegram.Message.damage;
        storedThrust = telegram.Message.thrust;
        storedDir = telegram.Message.dir;
    }

    public void OnProcessingMessage(Telegram<Player.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        GetDamage(storedDamage);
        Knockback(storedDir, storedThrust);
    }

    void GetDamage(float healthPoint)
    {
        if (loadPlayer.BarrierComponent.CanBarrierAbsorb() == true) return;

        if (loadPlayer.Data.Immortality == true) return;

        if (loadPlayer.Data.Hp > 0)
        {
            loadPlayer.Data.Hp -= healthPoint;
            if (loadPlayer.Data.Hp <= 0)
            {
                Die();
                loadPlayer.Data.Hp = 0;
            }
        }
    }

    void Die()
    {
        PlayManager.Instance.GameOver();
        loadPlayer.DestoryThis();
    }

    void Knockback(Vector2 dir, float thrust)
    {
        loadPlayer.DashComponent.CancelDash();
        loadPlayer.DashComponent.PlayDash(dir, thrust / loadPlayer.Data.Weight, loadPlayer.Data.StunTime);
    }



    public void OperateExit()
    {
    }

    public void OperateUpdate()
    {
        CheckSwitchStates();
    }
}
