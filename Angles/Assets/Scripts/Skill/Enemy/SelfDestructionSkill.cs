using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class SelfDestructionSkill : BasicSkill
{
    public Color color;
    public float radius;

    public override void PlayBasicSkill(Transform tr)
    {
        effect.PlayEffect();

        transform.position = tr.position; // 위치 초기화

        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < hit.Length; i++)
        {
            if (CheckCanHitSkill(hit[i].transform.tag) == false) continue;
            DamageToEntity(hit[i].transform.gameObject);
        }

        SkillTask().Forget();
    }

    public async UniTaskVoid SkillTask()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(disableTime), cancellationToken: source.Token);
        DisableObject();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
