using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class MultipleTickSpawnSkill : TickSpawnSkill
{
    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
        base.Execute(caster);
        SpawnTask(caster).Forget();
    }

    private async UniTaskVoid SpawnTask(GameObject caster)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(Data.PreDelay), cancellationToken: m_source.Token);

        //print("PreDelay");

        int storedTick = 0;

        float delay = Data.Duration / Data.TickCount;

        while (Data.TickCount > storedTick)
        {
            //print("Spawn");
            storedTick++;
            spawnMethod.Execute(new SpawnSupportData(caster, this, storedTick));
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: m_source.Token);
        }
    }
}
