using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PreDelaySkill : TickAttackSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public override void Execute(GameObject caster) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        // --> 위치 지정 로직 추가
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
