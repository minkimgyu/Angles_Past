using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateYellowTriangleAttack : StateFollowEnemyAttack
{
    YellowTriangleEnemy enemy;

    public StateYellowTriangleAttack(YellowTriangleEnemy yellowTriangle) : base(yellowTriangle)
    {
        enemy = yellowTriangle;
    }

    public override void ExecuteInRangeMethod()
    {
        List<BuffData> tmpData = enemy.Data.GrantedUtilization.LootBuffFromDB();

        for (int i = 0; i < tmpData.Count; i++)
        {
            if (enemy.LoadPlayer.BuffComponent.AddBuff(tmpData[i]) == false) return;

            BasicEffectPlayer effectPlayer = enemy.EffectMethod.ReturnEffectFromPool();
            if (effectPlayer == null) return;

            enemy.EffectPlayer = effectPlayer;
            enemy.EffectPlayer.Init(enemy.transform, 1000f);
            enemy.EffectPlayer.PlayEffect();

            Debug.Log("In");
        }
    }

    public override void ExecuteInOutsideMethod()
    {
        List<BuffData> tmpData = enemy.Data.GrantedUtilization.LootBuffFromDB();

        for (int i = 0; i < tmpData.Count; i++)
        {
            enemy.LoadPlayer.BuffComponent.RemoveBuff(tmpData[i]);

            if (enemy.EffectPlayer == null) return;
            enemy.EffectPlayer.StopEffect();

            Debug.Log("Out");
        }
    }
}
