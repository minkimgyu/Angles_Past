using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class KnockBackSkill : BasicSkill
{
    public Color color;
    public float boxWidth;
    public float boxHeight;
    public Vector3 distanceFromPlayer;

    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        transform.position = skillSupportData.player.transform.position;
        effect.PlayEffect();

        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(boxWidth, boxHeight), 
            transform.rotation.z, Vector2.right, distanceFromPlayer.x, LayerMask.GetMask("Enemy"));

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
        Gizmos.DrawWireCube(Vector3.zero + distanceFromPlayer, new Vector3(boxWidth, boxHeight, 0));
    }
}