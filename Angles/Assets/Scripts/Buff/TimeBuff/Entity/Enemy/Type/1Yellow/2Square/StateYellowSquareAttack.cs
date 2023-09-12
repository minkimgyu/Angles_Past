using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateYellowSquareAttack : StateFollowEnemyAttack
{
    public StateYellowSquareAttack(YellowSquareEnemy yellowSquareEnemy) : base(yellowSquareEnemy)
    {
    }

    public override void ExecuteInRangeMethod()
    {
        loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);
    }

    public override void ExecuteInOutsideMethod()
    {
        
    }
}
