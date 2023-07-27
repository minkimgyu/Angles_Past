using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateYellowSquareAttack : StateFollowEnemyAttack
{
    YellowSquareEnemy loadYellowSquareEnemy;

    public StateYellowSquareAttack(YellowSquareEnemy yellowSquareEnemy) : base(yellowSquareEnemy)
    {
        loadYellowSquareEnemy = yellowSquareEnemy;
    }

    public override void ExecuteInRangeMethod()
    {
        loadYellowSquareEnemy.BattleComponent.UseSkill(SkillUseConditionType.InRange);
    }

    public override void ExecuteInOutsideMethod()
    {
        
    }
}
