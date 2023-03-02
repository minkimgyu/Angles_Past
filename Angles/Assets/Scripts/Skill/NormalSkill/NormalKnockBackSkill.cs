using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class NormalKnockBackSkill : BasicSkill
{
    public override void PlaySkill(SkillSupportData data)
    {
        base.PlaySkill(data);
        transform.position = data.contactPos[0];
        effect.PlayEffect();

        for (int i = 0; i < data.contactEntity.Count; i++)
        {
            if (SkillData.CanHitSkill(data.contactEntity[i].tag) == false) continue;
          
            float ratio = DatabaseManager.Instance.PlayerData.StoredRushRatio;
            DamageToEntity(data.contactEntity[i].gameObject, SkillData.KnockBackThrust * ratio);
        }

        SkillTask().Forget();
    }

    public async UniTaskVoid SkillTask()
    {
        BasicTask.NowRunning = true;

        await UniTask.Delay(TimeSpan.FromSeconds(SkillData.DisableTime), cancellationToken: BasicTask.source.Token);
        DisableObject();

        BasicTask.NowRunning = false;
    }
}
