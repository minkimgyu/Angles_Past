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
    }

    public override void ExecuteInRangeMethod()
    {
        //List<BuffData> tmpData = enemy.HealthData.GrantedUtilization.LootBuffFromDB(); // --> 스킬로 변경

        //for (int i = 0; i < tmpData.Count; i++)
        //{
        //    BaseBuff tmpBuff = enemy.LoadPlayer.BuffComponent.AddBuff(tmpData[i]);
        //    if (tmpBuff == null) continue;
        //    else storedBuff.Add(tmpBuff);
        //}

        // 버프 넣는 부분 스킬로 수정

        //BasicEffectPlayer effectPlayer = enemy.EffectMethod.ReturnEffectFromPool();
        //if (effectPlayer == null) return;

        //enemy.EffectPlayer = effectPlayer;
        //enemy.EffectPlayer.Init(enemy.transform);
        //enemy.EffectPlayer.PlayEffect();
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
    }

    void AddEffect()
    {
        //BasicEffectPlayer effectPlayer = enemy.EffectMethod.ReturnEffectFromPool();
        //if (effectPlayer == null) return;

        //enemy.EffectPlayer = effectPlayer;
        //enemy.EffectPlayer.Init(enemy.transform, 1000f);
        //enemy.EffectPlayer.PlayEffect();
    }

    void RemoveEffect()
    {
        if (enemy.EffectPlayer == null) return;
        enemy.EffectPlayer.StopEffect();
    }

    public virtual void ReceiveOnDisable()
    {
        ExecuteInRangeMethod();

        if (enemy.EffectPlayer != null)
        {
            enemy.EffectPlayer.StopEffect();
            enemy.EffectPlayer = null;
        }
    }
}
