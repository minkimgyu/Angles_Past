using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBossRushEnemyGlobal : StateFollowEnemyAttack
{
    BossRushEnemy loadBossEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    public StateBossRushEnemyGlobal(BossRushEnemy bossEnemy) : base(bossEnemy)
    {
        loadBossEnemy = bossEnemy;
    }

    public override void ReceiveCollisionEnter(Collision2D collision)
    {
        loadBossEnemy.ContactComponent.CallWhenCollisionEnter(collision);
    }

    public override void ReceiveCollisionExit(Collision2D collision)
    {
        loadBossEnemy.ContactComponent.CallWhenCollisionExit(collision);
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false; // 여기에서 state 변화 주기
    }

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

                loadBossEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);
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
}
