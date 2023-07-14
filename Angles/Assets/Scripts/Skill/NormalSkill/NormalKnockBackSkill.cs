using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class NormalKnockBackSkill : MonoBehaviour
{
    //public override void PlaySkill(SkillSupportData data, BasicBattleComponent battleComponent)
    //{
    //    base.PlaySkill(data, battleComponent);

    //    for (int i = 0; i < data.contactEntity.Count; i++)
    //    {
    //        if (SkillData.CanHitSkill(data.contactEntity[i].tag) == false) continue;
    //        DamageToEntity(data.contactEntity[i].gameObject, SkillData.KnockBackThrust * data.playerDir.magnitude);
    //    }

    //    SkillTask().Forget();
    //}

    //public async UniTaskVoid SkillTask()
    //{
    //    BasicTask.NowRunning = true;

    //    await UniTask.Delay(TimeSpan.FromSeconds(SkillData.DisableTime), cancellationToken: BasicTask.source.Token);
    //    DisableObject();

    //    BasicTask.NowRunning = false;
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();

    //}
}
