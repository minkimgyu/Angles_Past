using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StickyBombSkill : BasicSkill
{
    public Color color;

    Transform explosionTr;
    Vector3 dirOnContact;

    public async UniTaskVoid SkillTask()
    {
        BasicTask.NowRunning = true;
        await UniTask.Delay(TimeSpan.FromSeconds(SkillData.PreDelay), cancellationToken: BasicTask.source.Token);
        DamageToRange();
        effect.PlayEffect();

        BasicTask.NowRunning = false;


        await UniTask.Delay(TimeSpan.FromSeconds(SkillData.DisableTime), cancellationToken: BasicTask.source.Token);
        DisableObject();
    }

    public override void PlaySkill(SkillSupportData data)
    {
        base.PlaySkill(data);
        explosionTr = data.contactEntity[0].transform;
        dirOnContact = data.contactPos[0] - data.contactEntity[0].transform.position;
        SkillTask().Forget();
    }

    public void DamageToRange()
    {
        transform.position = explosionTr.position;

        Vector3 contactPos = transform.position + dirOnContact;
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, SkillData.RadiusRange, Vector2.up, 0, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (SkillData.CanHitSkill(hit[i].transform.tag) == false) continue;
            DamageToEntity(hit[i].transform.gameObject, SkillData.KnockBackThrust);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, SkillData.RadiusRange);
    }
}
