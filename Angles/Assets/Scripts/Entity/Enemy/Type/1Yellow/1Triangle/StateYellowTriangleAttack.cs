using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateYellowTriangleAttack : StateFollowEnemyAttack
{
    YellowTriangleEnemy loadyYellowTriangleEnemy;

    public StateYellowTriangleAttack(YellowTriangleEnemy yellowTriangle) : base(yellowTriangle)
    {
        loadyYellowTriangleEnemy = yellowTriangle;
    }

    public override void ExecuteInRangeMethod()
    {
        if (loadyYellowTriangleEnemy.LoadPlayer.BuffComponent.AddBuff("SpeedDebuff") == false) return;

        BasicEffectPlayer effectPlayer = loadyYellowTriangleEnemy.EffectMethod.ReturnEffectFromPool();
        if (effectPlayer == null) return;

        loadyYellowTriangleEnemy.EffectPlayer = effectPlayer;
        loadyYellowTriangleEnemy.EffectPlayer.Init(loadyYellowTriangleEnemy.transform, 1000f);
        loadyYellowTriangleEnemy.EffectPlayer.PlayEffect();

        Debug.Log("In");
    }

    public override void ExecuteInOutsideMethod()
    {
        loadyYellowTriangleEnemy.LoadPlayer.BuffComponent.RemoveBuff("SpeedDebuff");

        if (loadyYellowTriangleEnemy.EffectPlayer == null) return;
        loadyYellowTriangleEnemy.EffectPlayer.StopEffect();

        Debug.Log("Out");
    }
}
