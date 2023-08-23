using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReflectEnemyRush : BaseState<BaseReflectEnemy.State>
{
    BaseReflectEnemy enemy;

    public StateReflectEnemyRush(BaseReflectEnemy baseReflectEnemy)
    {
        enemy = baseReflectEnemy;
    }

    public override void OnMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
    }

    public override void OperateUpdate()
    {
        if(enemy.Rigidbody.velocity.magnitude < enemy.MinRushVec)
        {
            enemy.RushVec = enemy.ResetRushVec();
            enemy.DashComponent.QuickEndTask();
            enemy.DashComponent.PlayDash(enemy.RushVec.normalized, enemy.Speed.IntervalValue);
        }
    }

    public override void OperateEnter()
    {
        enemy.DashComponent.QuickEndTask();
        enemy.DashComponent.PlayDash(enemy.RushVec.normalized, enemy.Speed.IntervalValue);

    }

    public override void OperateExit()
    {
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

    public override void ReceiveCollisionEnter(Collision2D collision)
    {
        enemy.ContactComponent.CallWhenCollisionEnter(collision);
        GoToReflectState(collision);
    }

    public override void ReceiveCollisionExit(Collision2D collision)
    {
        enemy.ContactComponent.CallWhenCollisionExit(collision);
    }

    public override void ReceiveUnderAttack(float healthPoint, Vector2 dir, float thrust)
    {
        GoToGetDamageState(healthPoint, dir, thrust);
    }
}
