using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PreDelaySkill : TickSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public override void Execute(GameObject caster)
    {
        base.Execute(caster);
        DamageTask(new DamageSupportData(caster, gameObject, data)).Forget();
    }

    private async UniTaskVoid DamageTask(DamageSupportData damageSupportData)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(damageSupportData.Data.PreDelay), cancellationToken: m_source.Token);

        damageMethod.Attack(damageSupportData);
        gameObject.SetActive(false);
    }
}
