using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class MultipleTickSkill : TickAttackSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    [SerializeField]
    int storedTick;

    float storedTickTime;
    bool canDamage = true;

    public override void CancelSkill()
    {
        storedTickTime = 0;
        storedTick = 0;
        storedDelay = 0;
    }

    protected override void DamageTask(float tick)
    {
        storedDelay += tick;
        if (damageSupportData.Me.Data.PreDelay > storedDelay) return;

        float delay = damageSupportData.Me.Data.Duration / damageSupportData.Me.Data.TickCount;


        if (damageSupportData.Me.Data.TickCount > storedTick)
        {
            if(canDamage) damageMethod.Execute(damageSupportData);
            canDamage = false;
            storedTickTime += tick;
            if (delay > storedTickTime) return;



            damageMethod.Execute(damageSupportData);

            storedTickTime = 0;
            storedTick++;
            canDamage = true;
        }
        else
        {
            IsFinished = true;
        }
    }

    public override void OnEnd()
    {
        storedTickTime = 0;
        storedTick = 0;
        base.OnEnd();
    }


    //private async UniTaskVoid DamageTask(DamageSupportData damageSupportData)
    //{
    //    await UniTask.Delay(TimeSpan.FromSeconds(damageSupportData.Me.Data.PreDelay), cancellationToken: m_source.Token);

    //    int storedTick = 0;

    //    float delay = damageSupportData.Me.Data.Duration / damageSupportData.Me.Data.TickCount;

    //    while (damageSupportData.Me.Data.TickCount > storedTick)
    //    {
    //        damageMethod.Execute(damageSupportData);
    //        storedTick++;
    //        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: m_source.Token);
    //    }

    //    gameObject.SetActive(false);
    //}
}
