using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StickyBombSkill : BasicSkill
{
    public Color color;
    public float radius;
    public float explosionTime = 2f;

    Transform explosionTr;
    Vector3 dirOnContact;

    public async UniTaskVoid SkillTask()
    {
        nowRunning = true;
        await UniTask.Delay(TimeSpan.FromSeconds(explosionTime), cancellationToken: source.Token);
        DamageToRange();
        effect.PlayEffect();

        nowRunning = false;


        await UniTask.Delay(TimeSpan.FromSeconds(disableTime), cancellationToken: source.Token);
        DisableObject();
    }

    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        explosionTr = skillSupportData.contactEntity[0].transform;
        dirOnContact = skillSupportData.contactPos[0] - skillSupportData.contactEntity[0].transform.position;
        SkillTask().Forget();
    }

    public void DamageToRange()
    {
        transform.position = explosionTr.position;

        Vector3 contactPos = transform.position + dirOnContact;
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (CheckCanHitSkill(hit[i].transform.tag) == false) continue;
            DamageToEntity(hit[i].transform.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
