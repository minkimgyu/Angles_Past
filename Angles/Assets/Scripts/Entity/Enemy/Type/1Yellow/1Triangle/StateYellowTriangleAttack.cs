using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateYellowTriangleAttack : StateFollowEnemyAttack
{
    YellowTriangleEnemy enemy;

    List<BaseBuff> storedBuff = new List<BaseBuff>();

    public StateYellowTriangleAttack(YellowTriangleEnemy yellowTriangle) : base(yellowTriangle)
    {
        enemy = yellowTriangle;
        yellowTriangle.WhenDisable += ExecuteInOutsideMethod;
    }

    public override void ExecuteInRangeMethod()
    {
        List<BuffData> tmpData = enemy.Data.GrantedUtilization.LootBuffFromDB();

        for (int i = 0; i < tmpData.Count; i++)
        {
            BaseBuff tmpBuff = enemy.LoadPlayer.BuffComponent.AddBuff(tmpData[i]);
            if (tmpBuff == null) continue;
            else storedBuff.Add(tmpBuff);
        }

        BasicEffectPlayer effectPlayer = enemy.EffectMethod.ReturnEffectFromPool();
        if (effectPlayer == null) return;

        enemy.EffectPlayer = effectPlayer;
        enemy.EffectPlayer.Init(enemy.transform, 1000f);
        enemy.EffectPlayer.PlayEffect();

        Debug.Log("In");
    }

    public override void ExecuteInOutsideMethod()
    {
        for (int i = 0; i < storedBuff.Count; i++)
        {
            enemy.LoadPlayer.BuffComponent.RemoveBuff(storedBuff[i]);
        }

        storedBuff.Clear();

        if (enemy.EffectPlayer == null) return;
        enemy.EffectPlayer.StopEffect();

        Debug.Log("Out");
    }
}
