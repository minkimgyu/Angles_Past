using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollowEnemyDamaged : BaseState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    float storedDamage;
    float storedThrust;
    Vector2 storedDir;

    public StateFollowEnemyDamaged(BaseFollowEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public override void CheckSwitchStates()
    {
        if (loadFollowEnemy.DashComponent.NowFinish == true)
        {
            loadFollowEnemy.RemoveEnemyContactKnockback();
            loadFollowEnemy.RevertToPreviousState();
        }
    }

    public override void OnMessage(Telegram<BaseFollowEnemy.State> telegram)
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
        loadFollowEnemy.DashComponent.CancelDash(false);
        loadFollowEnemy.DashComponent.PlayDash(dir, thrust / loadFollowEnemy.Weight.IntervalValue, loadFollowEnemy.StunTime.IntervalValue, false); // 스턴은 무계 비율에 맞게 조정하기

        loadFollowEnemy.AddEnemyContactKnockback();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}
