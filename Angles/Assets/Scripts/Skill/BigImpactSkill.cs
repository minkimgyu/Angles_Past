using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class BigImpactSkill : BasicSkill
{
    public Color color;
    public float radius;

    public override void PlaySkill(SkillSupportData data)
    {
        base.PlaySkill(data);
        effect.PlayEffect();

        transform.position = data.player.transform.position; // 위치 초기화

        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, layerMask);
        for (int i = 0; i < hit.Length; i++)
        {   
            if (SkillData.CanHitSkill(hit[i].transform.tag) == false) continue;
            DamageToEntity(hit[i].transform.gameObject, SkillData.KnockBackThrust);
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

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
