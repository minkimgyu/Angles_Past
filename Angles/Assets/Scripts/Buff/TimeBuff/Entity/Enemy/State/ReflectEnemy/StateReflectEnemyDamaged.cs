using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReflectEnemyDamaged : BaseState<BaseReflectEnemy.State>
{
    BaseReflectEnemy loadReflectEnemy;

    float storedDamage;
    float storedThrust;
    Vector2 storedDir;

    public StateReflectEnemyDamaged(BaseReflectEnemy followEnemy)
    {
        loadReflectEnemy = followEnemy;
    }

    public override void CheckSwitchStates()
    {
        if (loadReflectEnemy.DashComponent.NowFinish == true)
        {
            loadReflectEnemy.RemoveEnemyContactKnockback();
            loadReflectEnemy.RevertToPreviousState();
        }
    }

    public override void OnMessage(Telegram<BaseReflectEnemy.State> telegram)
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
        loadReflectEnemy.DashComponent.CancelDash(false);
        loadReflectEnemy.DashComponent.PlayDash(dir, thrust / loadReflectEnemy.Weight.IntervalValue, loadReflectEnemy.StunTime.IntervalValue, false); // 스턴은 무계 비율에 맞게 조정하기

        loadReflectEnemy.AddEnemyContactKnockback();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
        CheckSwitchStates();
    }
}
