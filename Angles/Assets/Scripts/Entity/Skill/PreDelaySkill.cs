using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PreDelaySkill : TickSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public override void Execute(GameObject caster)
    {
        base.Execute(caster);
        DamageTask(new DamageSupportData(caster, this)).Forget();
    }

    private async UniTaskVoid DamageTask(DamageSupportData damageSupportData)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(damageSupportData.Me.Data.PreDelay), cancellationToken: m_source.Token);

        damageMethod.Execute(damageSupportData);
        gameObject.SetActive(false);
    }
}
