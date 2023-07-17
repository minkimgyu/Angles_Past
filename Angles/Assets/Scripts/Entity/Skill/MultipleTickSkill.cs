using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class MultipleTickSkill : TickSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
        base.Execute(caster);
        DamageTask(new DamageSupportData(caster, gameObject, data)).Forget();
    }

    private async UniTaskVoid DamageTask(DamageSupportData damageSupportData)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(damageSupportData.Data.PreDelay), cancellationToken: m_source.Token);

        int storedTick = 0;

        float delay = damageSupportData.Data.Duration / damageSupportData.Data.TickCount;

        while (damageSupportData.Data.TickCount > storedTick)
        {
            damageMethod.Attack(damageSupportData);
            storedTick++;
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: m_source.Token);
        }

        gameObject.SetActive(false);
    }
}
