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

    public async UniTaskVoid SkillTask(Transform tr)
    {
        NowRunning = true;
        int damageTic = 0;

        int maxDamageTic = (int)(damageTime / damagePerTime);

        while (damageTic < maxDamageTic)
        {
            AttackCircleRange(tr);
            await UniTask.Delay(TimeSpan.FromSeconds(damagePerTime), cancellationToken: source.Token);
            damageTic += 1;
        }

        NowRunning = false;

        DisableObject();
        // 이펙트를 꺼주는 코드 추가
    }

    private void Update()
    {
        if (moveTr == null) return;

        transform.position = moveTr.position;
        transform.rotation = moveTr.rotation;
    }

    protected override void DisableObject()
    {
        ObjectPooler.ReturnToPool(basicEffect.gameObject, true);
        basicEffect.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    void AttackCircleRange(Transform tr)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                hit[i].collider.GetComponent<FollowComponent>().WaitFollow();
                hit[i].collider.GetComponent<BasicReflectComponent>().KnockBack((hit[i].transform.position - tr.position).normalized * 1.5f);
            }
        }
    }

    public override void PlaySkill(Vector2 dir, List<Collision2D> entity)
    {
        SkillTask(moveTr).Forget();
        GetEffectUsingName(transform.position, transform.rotation, transform);

        base.PlaySkill(dir, entity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
