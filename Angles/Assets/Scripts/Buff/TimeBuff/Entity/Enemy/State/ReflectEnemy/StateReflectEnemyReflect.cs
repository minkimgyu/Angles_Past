using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateReflectEnemyReflect : BaseState<BaseReflectEnemy.State>
{
    BaseReflectEnemy enemy;
    Vector2 savedReflectVec;

    Dictionary<EntityTag, Action<Telegram<BaseReflectEnemy.State>>> ReflectMethod;

    public StateReflectEnemyReflect(BaseReflectEnemy baseReflectEnemy)
    {
        enemy = baseReflectEnemy;

        ReflectMethod = new Dictionary<EntityTag, Action<Telegram<BaseReflectEnemy.State>>>()
        {
            { EntityTag.Player, (Telegram<BaseReflectEnemy.State> telegram) => 
                {
                    enemy.SkillController.UseSkill(BaseSkill.UseConditionType.Contact);
                    ReturnRushVec(telegram);
                } 
            },
            { EntityTag.Wall, (Telegram<BaseReflectEnemy.State> telegram) => { ReturnRushVec(telegram); } }
        };
    }

    void ReturnRushVec(Telegram<BaseReflectEnemy.State> telegram)
    {
        Vector2 tmpDir = enemy.ReflectComponent.ResetReflectVec(telegram.Message.dir);

        if (tmpDir == Vector2.zero) enemy.RushVec = enemy.ResetRushVec();
        else enemy.RushVec = tmpDir;
    }


    public override void OnMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
        if (telegram.SenderStateName != BaseReflectEnemy.State.Rush) return;

        ReflectMethod[telegram.Message.contactedEntity.InheritedTag](telegram);
    }

    public override void OperateEnter()
    {
        enemy.RevertToPreviousState();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}
