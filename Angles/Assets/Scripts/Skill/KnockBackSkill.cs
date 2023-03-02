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

    public override void PlaySkill(SkillSupportData data)
    {
        base.PlaySkill(data);
        transform.position = data.player.transform.position;
        effect.PlayEffect();

        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(boxWidth, boxHeight), 
            transform.rotation.z, Vector2.right, distanceFromPlayer.x, LayerMask.GetMask("Enemy"));

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
        Gizmos.DrawWireCube(Vector3.zero + distanceFromPlayer, new Vector3(boxWidth, boxHeight, 0));
    }
}