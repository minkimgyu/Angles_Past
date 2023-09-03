using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class BladeSkill //: BasicSkill
{
    //public Color color;
    //public float radius;
    //Transform playerTr = null;

    //public override void PlayAddition()
    //{
    //    base.PlayAddition();
    //    BasicTask.CancelTask();
    //    effect.StopEffect();

    //    if (playerTr == null) return;
    //    SkillTask().Forget();
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    if (playerTr != null) playerTr = null;
    //}

    //public async UniTaskVoid SkillTask()
    //{
    //    effect.PlayEffect();

    //    BasicTask.NowRunning = true;
    //    int damageTic = 0;

    //    int maxDamageTic = (int)(SkillData.Duration / SkillData.UseTick);

    //    while (damageTic < maxDamageTic)
    //    {
    //        AttackCircleRange();
    //        await UniTask.Delay(TimeSpan.FromSeconds(SkillData.UseTick), cancellationToken: BasicTask.source.Token);
    //        damageTic += 1;
    //    }

    //    effect.StopEffect();

    //    await UniTask.Delay(TimeSpan.FromSeconds(SkillData.DisableTime), cancellationToken: BasicTask.source.Token);
    //    BasicTask.NowRunning = false;
    //    DisableObject();
    //}

    //private void Update()
    //{
    //    if (playerTr == null) return;

    //    transform.position = playerTr.position;
    //}

    //void AttackCircleRange()
    //{
    //    RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0);

    //    for (int i = 0; i < hit.Length; i++)
    //    {
    //        if (SkillData.CanHitSkill(hit[i].transform.tag) == false) continue;
    //        DamageToEntity(hit[i].transform.gameObject, SkillData.KnockBackThrust);
    //    }
    //}

    //public override void PlaySkill(SkillSupportData data, BasicBattleComponent battleComponent)
    //{
    //    base.PlaySkill(data, battleComponent);
    //    playerTr = data.playerTransform.transform; // 위치 초기화
    //    SkillTask().Forget();
    //}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = color;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}
}
