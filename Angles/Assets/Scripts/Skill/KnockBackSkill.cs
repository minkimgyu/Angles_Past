using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class KnockBackSkill : BasicSkill
{
    public Color color;
    public ParticleSystem pushEffect;

    public override void PlaySkill(SkillSupportData data, BasicBattleComponent battleComponent)
    {
        base.PlaySkill(data, battleComponent);
        transform.position = data.player.transform.position;
        transform.rotation = data.player.transform.rotation;

        float tempRotation = transform.eulerAngles.z;
        print(tempRotation);

        var main = pushEffect.main;
        main.startRotation = -tempRotation * Mathf.Deg2Rad;

        effect.PlayEffect();

        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, SkillData.BoxRange, 
            transform.rotation.z, Vector2.right, SkillData.OffsetRange.magnitude, LayerMask.GetMask("Enemy"));

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
        Gizmos.DrawWireCube(SkillData.OffsetRange, SkillData.BoxRange);
    }
}