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
            ExitStun();
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Follow);
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

    void ExitStun()
    {
        SkillData skillData = DatabaseManager.Instance.UtilizationDB.ReturnSkillData("EnemyContactKnockback");
        loadFollowEnemy.BattleComponent.RemoveSkillFromPossessingSkills(skillData); // 만약 안 쓰고 존재한다면 삭제해준다.
        //Debug.Log("ExitStun");
    }

    void Knockback(Vector2 dir, float thrust)
    {
        loadFollowEnemy.DashComponent.CancelDash(false);
        loadFollowEnemy.DashComponent.PlayDash(dir, thrust / loadFollowEnemy.HealthData.Weight, loadFollowEnemy.HealthData.StunTime, false); // 스턴은 무계 비율에 맞게 조정하기

        SkillData skillData = DatabaseManager.Instance.UtilizationDB.ReturnSkillData("EnemyContactKnockback");
        loadFollowEnemy.BattleComponent.LootingSkill(skillData);
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
    }
}
