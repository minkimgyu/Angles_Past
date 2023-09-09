using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBossRushEnemyAttack : StateFollowEnemyAttack
{
    BossRushEnemy loadBossEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    public StateBossRushEnemyAttack(BossRushEnemy bossEnemy) : base(bossEnemy)
    {
        loadBossEnemy = bossEnemy;
    }

    public override void ReceiveCollisionEnter(Collision2D collision) 
    {
        loadBossEnemy.ContactComponent.CallWhenCollisionEnter(collision);
        loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.Contact);
    }

    public override void ReceiveCollisionExit(Collision2D collision) 
    {
        loadBossEnemy.ContactComponent.CallWhenCollisionExit(collision);
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false;
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
                Vector3 tmpDir = (loadBossEnemy.LoadPlayer.transform.position - loadBossEnemy.transform.position).normalized;
                loadBossEnemy.DashComponent.PlayDash(tmpDir, loadBossEnemy.AttackThrust.IntervalValue); // 방향은 랜덤으로
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
