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

        if(attackFlag)
        {
            if(canAttack == true)
            {
                // 첫번째 레이저 스킬
                loadYellowOctagonEnemy.BattleComponent.PossessingSkills[0].Directions = ReturnRandomDirection();

                // state를 스킬 사용 시 --> 정지 --> 추적으로 바꿔줌
                loadYellowOctagonEnemy.BattleComponent.UseSkill(SkillUseConditionType.InRange);
                canAttack = false;
            }
        }

        if(canAttack == false)
        {
            storedTime += Time.deltaTime;
            if(storedTime > loadYellowOctagonEnemy.Delay)
            {
                storedTime = 0;
                canAttack = true;
            }
        }
    }

    List<Vector3> ReturnRandomDirection()
    {
        List<Vector3> directions = new List<Vector3>();

        for (int i = 0; i < loadYellowOctagonEnemy.DirectionCount; i++)
        {
            float angle = Random.Range(0, 361);
            directions.Add(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0));
        }

        return directions;
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false;
    }
}