using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StateYellowPentagonAttack : StateFollowEnemyAttack
{
    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    BuffFloat attackDelay;

    public StateYellowPentagonAttack(YellowPentagonEnemy yellowPentagonEnemy) : base(yellowPentagonEnemy)
    {
        attackDelay = yellowPentagonEnemy.AttackDelay;
    }

    // 계속 발사해야해서 unitask로 제작      
    public override void ExecuteInRangeMethod()
    {
        attackFlag = true;
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        if(attackFlag)
        {
            if(canAttack == true)
            {
                loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);
                canAttack = false;
            }
        }

        if(canAttack == false)
        {
            storedTime += Time.deltaTime;
            if(storedTime > attackDelay.IntervalValue)
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
