using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PreDelaySkill : TickAttackSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
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

        damageMethod.Execute(new DamageSupportData(caster, this, 1));
        OnEnd();
    }
}
