using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PreDelaySkill : TickAttackSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public override void CancelSkill()
    {
    }

    protected override void DamageTask(float tick)
    {
        storedDelay += tick;
        if (damageSupportData.Me.Data.PreDelay > storedDelay) return;

        damageMethod.Execute(damageSupportData);
        IsFinished = true;
    }

    //private async UniTaskVoid DamageTask(DamageSupportData damageSupportData)
    //{
    //    print("DamageTask");
    //    await UniTask.Delay(TimeSpan.FromSeconds(damageSupportData.Me.Data.PreDelay), cancellationToken: m_source.Token);

    //    print("DamageTask12");

    //    damageMethod.Execute(damageSupportData);
    //    gameObject.SetActive(false);
    //}
}
