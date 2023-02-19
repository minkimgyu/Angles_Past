using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class BladeSkill : BasicSkill
{
    public Color color;
    public float radius;
    float damageTime = 5f;
    float damagePerTime = 0.5f; // 10
    BasicEffect basicEffect;
    Transform playerTr = null;

    public async UniTaskVoid SkillTask()
    {
        effect.PlayEffect();

        NowRunning = true;
        int damageTic = 0;

        int maxDamageTic = (int)(damageTime / damagePerTime);

        while (damageTic < maxDamageTic)
        {
            AttackCircleRange();
            await UniTask.Delay(TimeSpan.FromSeconds(damagePerTime), cancellationToken: source.Token);
            damageTic += 1;
        }

        NowRunning = false;

        effect.StopEffect();
    }

    private void Update()
    {
        if (playerTr == null) return;

        transform.position = playerTr.position;
    }

    void AttackCircleRange()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                hit[i].collider.GetComponent<FollowComponent>().WaitFollow();
                hit[i].collider.GetComponent<BasicReflectComponent>().KnockBack((hit[i].transform.position - transform.position).normalized * 1.5f);
            }
        }
    }

    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        playerTr = skillSupportData.player.transform; // 위치 초기화
        SkillTask().Forget();
        base.PlaySkill(skillSupportData);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
