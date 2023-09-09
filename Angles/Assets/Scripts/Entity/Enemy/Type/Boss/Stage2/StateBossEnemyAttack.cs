using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBossEnemyAttack : StateFollowEnemyAttack
{
    BossEnemy loadBossEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    public StateBossEnemyAttack(BossEnemy bossEnemy) : base(bossEnemy)
    {
        loadBossEnemy = bossEnemy;
    }

    // 계속 발사해야해서 unitask로 제작      
    public override void ExecuteInRangeMethod()
    {
        attackFlag = true;
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        if (attackFlag)
        {
            if (canAttack == true)
            {
                loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);
                canAttack = false;
            }
        }

        if (canAttack == false)
        {
            storedTime += Time.deltaTime;
            if (storedTime > loadBossEnemy.AttackDelay.IntervalValue)
            {
                storedTime = 0;
                canAttack = true;
            }
        }
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false;
    }
}
