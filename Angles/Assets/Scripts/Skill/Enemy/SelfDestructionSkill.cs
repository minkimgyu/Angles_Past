using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class SelfDestructionSkill : MonoBehaviour
{
    //public Color color;
    //public Color beforeColor;
    //public Color afterColor;

    //SpriteRenderer sr;

    //float currentTime = 0;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    AddTask("ColorChangeTask");
    //}

    //public override void PlaySkill(Transform tr, BasicBattleComponent battleComponent)
    //{
    //    base.PlaySkill(tr, battleComponent);

    //    Enemy enemy = tr.GetComponent<Enemy>();
    //    enemy.dieAction += CancleSkill;

    //    sr = enemy.innerSprite;
    //    if (sr == null) return;

    //    ColorChangeTask(sr).Forget();
    //}

    //void CancleSkill()
    //{
    //    if (tasks.ContainsKey("ColorChangeTask") == true)
    //    {
    //        print("CancleSkill");
    //        tasks["ColorChangeTask"].CancelTask();
    //        SkillTask().Forget(); // ��ų ������Ʈ ����
    //    }
    //}

    //public async UniTaskVoid ColorChangeTask(SpriteRenderer sr)
    //{
    //    tasks["ColorChangeTask"].NowRunning = true;

    //    while (SkillData.PreDelay >= currentTime)
    //    {
    //        currentTime += Time.deltaTime;

    //        sr.color = Color.Lerp(sr.color, afterColor, currentTime / SkillData.PreDelay);
    //        await UniTask.Delay(TimeSpan.FromSeconds(tasks["ColorChangeTask"].WaitTime), cancellationToken: tasks["ColorChangeTask"].source.Token);
    //    }

    //    currentTime = 0;

    //    DamageToCloseEntity(sr.transform.position);
    //    tasks["ColorChangeTask"].NowRunning = false;
    //}

    //void DamageToCloseEntity(Vector3 pos)
    //{
    //    print("DamageToCloseEntity");

    //    transform.position = pos;
    //    effect.PlayEffect();

    //    RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, SkillData.RadiusRange, Vector2.up, 0, LayerMask.GetMask("Enemy"));
    //    for (int i = 0; i < hit.Length; i++)
    //    {
    //        if (SkillData.CanHitSkill(hit[i].transform.tag) == false) continue;
    //        DamageToEntity(hit[i].transform.gameObject, SkillData.KnockBackThrust);
    //    }

    //    SkillTask().Forget();
    //}

    //public async UniTaskVoid SkillTask()
    //{
    //    BasicTask.NowRunning = true;

    //    await UniTask.Delay(TimeSpan.FromSeconds(SkillData.DisableTime), cancellationToken: BasicTask.source.Token);
    //    sr.color = beforeColor;
    //    DisableObject();

    //    BasicTask.NowRunning = false;
    //}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = color;
    //    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
    //    Gizmos.DrawWireSphere(Vector3.zero, SkillData.RadiusRange);
    //}
}
