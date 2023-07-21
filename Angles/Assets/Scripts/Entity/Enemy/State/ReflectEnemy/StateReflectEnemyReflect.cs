using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReflectEnemyReflect : IState<BaseReflectEnemy.State>
{
    BaseReflectEnemy enemy;
    Vector2 savedReflectVec;

    public StateReflectEnemyReflect(BaseReflectEnemy baseReflectEnemy)
    {
        enemy = baseReflectEnemy;
    }

    public void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
        if (telegram.SenderStateName != BaseReflectEnemy.State.Rush) return;

        if (telegram.Message.contactedEntity.InheritedTag == EntityTag.Player)
        {
            enemy.BattleComponent.UseSkill(SkillUseConditionType.Contact);

            Vector2 tmpDir = enemy.ReflectComponent.ResetReflectVec(telegram.Message.dir);

            if (tmpDir == Vector2.zero) enemy.RushVec = enemy.ResetRushVec();
            else enemy.RushVec = tmpDir;
        }
        else if(telegram.Message.contactedEntity.InheritedTag == EntityTag.Wall)
        {
            Vector2 tmpDir = enemy.ReflectComponent.ResetReflectVec(telegram.Message.dir);

            if (tmpDir == Vector2.zero) enemy.RushVec = enemy.ResetRushVec();
            else enemy.RushVec = tmpDir;
        }
    }

    public void OnProcessingMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        enemy.RevertToPreviousState();
    }

    public void OperateExit()
    {
    }

    public void OperateUpdate()
    {
    }
}
