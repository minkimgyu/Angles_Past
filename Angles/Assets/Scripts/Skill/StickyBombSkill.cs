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
        NowRunning = true;
        await UniTask.Delay(TimeSpan.FromSeconds(explosionTime), cancellationToken: source.Token);
        DamageToRange();
        effect.PlayEffect();

        NowRunning = false;
    }

    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        dirOnContact = skillSupportData.contactPos[0] - skillSupportData.contactEntity[0].transform.position;
        SkillTask().Forget();
    }

    public void DamageToRange()
    {
        Vector3 contactPos = transform.position + dirOnContact;

        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                hit[i].collider.GetComponent<FollowComponent>().WaitFollow();
                hit[i].collider.GetComponent<BasicReflectComponent>().KnockBack((hit[i].transform.position - contactPos).normalized * 2);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
