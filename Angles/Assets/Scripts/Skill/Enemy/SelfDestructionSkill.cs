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
    public Color beforeColor;
    public Color afterColor;

    protected override void Awake()
    {
        base.Awake();
        AddTask("ColorChangeTask");
        BasicTask.WaitTime = SkillData.DisableTime;
    }

    public override void PlayBasicSkill(Transform tr)
    {
        SpriteRenderer sr = tr.GetComponent<Enemy>().innerImage;
        ColorChangeTask(sr).Forget();
    }

    float smoothness = 0.01f;
    float duration = 3f;

    public async UniTaskVoid ColorChangeTask(SpriteRenderer sr)
    {
        tasks["ColorChangeTask"].NowRunning = true;

        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            sr.color = Color.Lerp(sr.color, afterColor, progress);
            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(tasks["ColorChangeTask"].WaitTime), cancellationToken: tasks["ColorChangeTask"].source.Token);
        }

        DamageToCloseEntity(sr.transform.position);
        tasks["ColorChangeTask"].NowRunning = false;
    }

    void DamageToCloseEntity(Vector3 pos)
    {
        transform.position = pos;
        effect.PlayEffect();

        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));
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

        await UniTask.Delay(TimeSpan.FromSeconds(BasicTask.WaitTime), cancellationToken: BasicTask.source.Token);
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
