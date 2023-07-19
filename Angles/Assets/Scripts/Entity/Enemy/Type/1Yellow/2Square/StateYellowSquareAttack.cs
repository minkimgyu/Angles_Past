using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateYellowSquareAttack : StateFollowEnemyAttack
{
    YellowSquareEnemy loadyYellowTriangleEnemy;

    public StateYellowSquareAttack(YellowSquareEnemy yellowSquareEnemy) : base(yellowSquareEnemy)
    {
        loadyYellowTriangleEnemy = yellowSquareEnemy;
    }

    public override void ExecuteInRangeMethod()
    {
       
    }

    public override void ExecuteInOutsideMethod()
    {
        
    }
}
