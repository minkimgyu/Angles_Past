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
        if (loadPlayer.DashComponent.NowFinish == true) loadPlayer.RevertToPreviousState();
    }

    public override void OnMessage(Telegram<Player.State> telegram)
    {
        storedDamage = telegram.Message.damage;
        storedThrust = telegram.Message.thrust;
        storedDir = telegram.Message.dir;
    }

    public override void OperateEnter()
    {
        //GetDamage(storedDamage);
        Knockback(storedDir, storedThrust);
    }

    //void GetDamage(float healthPoint)
    //{
    //    if (loadPlayer.BarrierComponent.CanBarrierAbsorb(healthPoint) == true) return;

    //    if (loadPlayer.Data.Immortality == true) return;

    //    if (loadPlayer.Data.Hp > 0)
    //    {
    //        loadPlayer.Data.Hp -= healthPoint;
    //        if (loadPlayer.Data.Hp <= 0)
    //        {
    //            Die();
    //            loadPlayer.Data.Hp = 0;
    //        }
    //    }
    //} 

    // --> 베리어랑 무적은 추후에 원본 클레서 가서 수정하기

    //void Die()
    //{
    //    PlayManager.Instance.GameOver();
    //    loadPlayer.DestoryThis();
    //}

    void Knockback(Vector2 dir, float thrust)
    {
        loadPlayer.DashComponent.CancelDash();
        loadPlayer.DashComponent.PlayDash(dir, thrust / loadPlayer.HealthData.Weight, loadPlayer.HealthData.StunTime);
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}
