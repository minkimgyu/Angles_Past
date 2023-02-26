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
        transform.position = data.contactPos[0];
        effect.PlayEffect();

        for (int i = 0; i < data.contactEntity.Count; i++)
        {
            if (CheckCanHitSkill(data.contactEntity[i].tag) == false) continue;

            Vector2 dirToEnemy = data.contactEntity[i].gameObject.transform.position - transform.position;
            data.contactEntity[i].GetHit(ReturnDamage(), dirToEnemy);
        }

        SkillTask().Forget();
    }

    public async UniTaskVoid SkillTask()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(disableTime), cancellationToken: source.Token);
        DisableObject();
    }
}
