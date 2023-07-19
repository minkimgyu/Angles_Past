using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class MultipleTickSkill : TickSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public override void Execute(GameObject caster) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        // --> 위치 지정 로직 추가
        base.Execute(caster);

        CancelTask(); // 먼저 돌리고 있는게 있다면 취소시키고 다시 실행 //--> 블레이드와 같은 스킬의 초기화

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
