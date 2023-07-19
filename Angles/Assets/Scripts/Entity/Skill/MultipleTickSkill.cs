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

        CancelTask(); // ���� ������ �ִ°� �ִٸ� ��ҽ�Ű�� �ٽ� ���� //--> ���̵�� ���� ��ų�� �ʱ�ȭ

        DamageTask(new DamageSupportData(caster, this)).Forget();
    }

    private async UniTaskVoid DamageTask(DamageSupportData damageSupportData)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(damageSupportData.Me.Data.PreDelay), cancellationToken: m_source.Token);

        int storedTick = 0;

        float delay = damageSupportData.Me.Data.Duration / damageSupportData.Me.Data.TickCount;

        while (damageSupportData.Me.Data.TickCount > storedTick)
        {
            damageMethod.Execute(damageSupportData);
            storedTick++;
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: m_source.Token);
        }

        gameObject.SetActive(false);
    }
}
