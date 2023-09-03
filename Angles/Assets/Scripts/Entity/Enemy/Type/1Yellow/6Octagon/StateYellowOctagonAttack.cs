using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class StateYellowOctagonAttack : StateFollowEnemyAttack
{
    YellowOctagonEnemy loadYellowOctagonEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    bool canRemoveSign = true;

    public StateYellowOctagonAttack(YellowOctagonEnemy yellowHexagonEnemy) : base(yellowHexagonEnemy)
    {
        loadYellowOctagonEnemy = yellowHexagonEnemy;
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
                loadYellowOctagonEnemy.ResetRandomDirection(out List<int> angles);
                loadYellowOctagonEnemy.ActiveExpectDirectionPoint(angles);

                // state를 스킬 사용 시 --> 정지 --> 추적으로 바꿔줌
                loadYellowOctagonEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);
                canAttack = false;
            }
        }

        if (canAttack == false)
        {
            storedTime += Time.deltaTime;

            if(storedTime > loadYellowOctagonEnemy.AttackDelay.IntervalValue / 4 && canRemoveSign)
            {
                loadYellowOctagonEnemy.OffExpectDirectionPoint();
                canRemoveSign = false;
            }

            if (storedTime > loadYellowOctagonEnemy.AttackDelay.IntervalValue)
            {
                loadYellowOctagonEnemy.CancelInvoke();
                storedTime = 0;
                canRemoveSign = canAttack = true;
            }
        }
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false;
    }
}