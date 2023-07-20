using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PreDelaySkill : TickAttackSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
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
