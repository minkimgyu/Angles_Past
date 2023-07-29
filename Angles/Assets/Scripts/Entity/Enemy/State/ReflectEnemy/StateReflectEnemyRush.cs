using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReflectEnemyRush : IState<BaseReflectEnemy.State>
{
    BaseReflectEnemy enemy;

    public StateReflectEnemyRush(BaseReflectEnemy baseReflectEnemy)
    {
        enemy = baseReflectEnemy;
    }

    public void CheckSwitchStates()
    {
    }

    public void OnAwakeMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
    }

    public void OnProcessingMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateUpdate()
    {
        if(enemy.Rigid.velocity.magnitude < enemy.MinRushVec)
        {
            enemy.RushVec = enemy.ResetRushVec();
            enemy.DashComponent.QuickEndTask();
            enemy.DashComponent.PlayDash(enemy.RushVec.normalized, enemy.Data.Speed.IntervalValue);
        }
    }

    public void OperateEnter()
    {
        enemy.ContactAction += GoToReflectState;
        enemy.UnderAttackAction += GoToGetDamageState;

        enemy.DashComponent.QuickEndTask();
        enemy.DashComponent.PlayDash(enemy.RushVec.normalized, enemy.Data.Speed.IntervalValue);

    }

    public void OperateExit()
    {
        enemy.ContactAction -= GoToReflectState;
        enemy.UnderAttackAction -= GoToGetDamageState;
    }

    void GoToGetDamageState(float damage, Vector2 dir, float thrust)
    {
        Message<BaseReflectEnemy.State> message = new Message<BaseReflectEnemy.State>();
        message.dir = dir;
        message.damage = damage;
        message.thrust = thrust;

        Telegram<BaseReflectEnemy.State> telegram = new Telegram<BaseReflectEnemy.State>(BaseReflectEnemy.State.Damaged, message);
        enemy.SetState(BaseReflectEnemy.State.Damaged, telegram);
    }

    public void GoToReflectState(Collision2D col)
    {
        col.gameObject.TryGetComponent(out Entity entity);
        if (entity == null) return;

        if (entity.InheritedTag == EntityTag.Player || entity.InheritedTag == EntityTag.Wall)
        {
            Message<BaseReflectEnemy.State> message = new Message<BaseReflectEnemy.State>();
            message.contactedEntity = entity;
            message.dir = col.contacts[0].normal;
            Telegram<BaseReflectEnemy.State> telegram = new Telegram<BaseReflectEnemy.State>(BaseReflectEnemy.State.Rush, BaseReflectEnemy.State.Reflect, message);

            enemy.SetState(BaseReflectEnemy.State.Reflect, telegram);
        }
    }
}
