using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollowEnemyDamaged : IState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    float storedDamage;
    float storedThrust;
    Vector2 storedDir;

    public StateFollowEnemyDamaged(BaseFollowEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public void CheckSwitchStates()
    {
        if (loadFollowEnemy.DashComponent.NowFinish == true)
        {
            ExitStun();
            loadFollowEnemy.RevertToPreviousState();
        }
    }

    public void OnAwakeMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
        storedDamage = telegram.Message.damage;
        storedThrust = telegram.Message.thrust;
        storedDir = telegram.Message.dir;
    }

    public void OnProcessingMessage(Telegram<BaseFollowEnemy.State> telegram)
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
        loadFollowEnemy.BattleComponent.RemoveSkillFromPossessingSkills(skillData); // ���� �� ���� �����Ѵٸ� �������ش�.
        //Debug.Log("ExitStun");
    }

    void Knockback(Vector2 dir, float thrust)
    {
        loadFollowEnemy.DashComponent.CancelDash(false);
        loadFollowEnemy.DashComponent.PlayDash(dir, thrust / loadFollowEnemy.Data.Weight, loadFollowEnemy.Data.StunTime, false); // ������ ���� ������ �°� �����ϱ�

        SkillData skillData = DatabaseManager.Instance.UtilizationDB.ReturnSkillData("EnemyContactKnockback");
        loadFollowEnemy.BattleComponent.LootingSkill(skillData);
    }

    public void OperateExit()
    {
    }

    public void OperateUpdate()
    {
        CheckSwitchStates();
    }
}
