using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class MultipleTickSkill : TickAttackSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
        base.Execute(caster);
        DamageTask(caster).Forget();
    }

    private async UniTaskVoid DamageTask(GameObject caster)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(Data.PreDelay), cancellationToken: m_source.Token);

        int storedTick = 0;

        float delay = Data.Duration / Data.TickCount;

        while (Data.TickCount > storedTick)
        {
            storedTick++;
            damageMethod.Execute(new DamageSupportData(caster, this, storedTick));
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: m_source.Token);
        }

        OnEnd();
    }
}
