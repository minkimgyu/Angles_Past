using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReflectEnemyDamaged : IState<BaseReflectEnemy.State>
{
    BaseReflectEnemy loadReflectEnemy;

    float storedDamage;
    float storedThrust;
    Vector2 storedDir;

    public StateReflectEnemyDamaged(BaseReflectEnemy followEnemy)
    {
        loadReflectEnemy = followEnemy;
    }

    public void CheckSwitchStates()
    {
        if (loadReflectEnemy.DashComponent.NowFinish == true)
        {
            ExitStun();
            loadReflectEnemy.RevertToPreviousState();
        }
    }

    public void OnAwakeMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
        storedDamage = telegram.Message.damage;
        storedThrust = telegram.Message.thrust;
        storedDir = telegram.Message.dir;
    }

    public void OnProcessingMessage(Telegram<BaseReflectEnemy.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        Knockback(storedDir, storedThrust);
    }

    void ExitStun()
    {
        SkillData skillData = DatabaseManager.Instance.UtilizationDB.ReturnSkillData("EnemyContactKnockback");
        loadReflectEnemy.BattleComponent.RemoveSkillFromPossessingSkills(skillData); // 만약 안 쓰고 존재한다면 삭제해준다.
        //Debug.Log("ExitStun");
    }

    void Knockback(Vector2 dir, float thrust)
    {
        loadReflectEnemy.DashComponent.CancelDash(false);
        loadReflectEnemy.DashComponent.PlayDash(dir, thrust / loadReflectEnemy.Data.Weight, loadReflectEnemy.Data.StunTime, false); // 스턴은 무계 비율에 맞게 조정하기

        SkillData skillData = DatabaseManager.Instance.UtilizationDB.ReturnSkillData("EnemyContactKnockback");
        loadReflectEnemy.BattleComponent.LootingSkill(skillData);
    }

    public void OperateExit()
    {
    }

    public void OperateUpdate()
    {
        CheckSwitchStates();
    }
}
