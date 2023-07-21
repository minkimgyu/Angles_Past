using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StateYellowHexaagonAttack : StateFollowEnemyAttack
{
    YellowHexagonEnemy loadYellowHexagonEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    public StateYellowHexaagonAttack(YellowHexagonEnemy yellowHexagonEnemy) : base(yellowHexagonEnemy)
    {
        loadYellowHexagonEnemy = yellowHexagonEnemy;
    }

    // ��� �߻��ؾ��ؼ� unitask�� ����      
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
                // state�� ��ų ��� �� --> ���� --> �������� �ٲ���
                loadYellowHexagonEnemy.BattleComponent.UseSkill(SkillUseConditionType.InRange);
                canAttack = false;
            }
        }

        if(canAttack == false)
        {
            storedTime += Time.deltaTime;
            if(storedTime > loadYellowHexagonEnemy.Delay)
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
