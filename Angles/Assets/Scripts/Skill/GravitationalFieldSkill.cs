using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class GravitationalFieldSkill : MonoBehaviour
{
    //[SerializeField]
    //List<Enemy> enemyList = new List<Enemy>();
    //float absorbThrust;

    //protected override void OnEnable()
    //{
    //    WhenEnable();
    //}

    //public override void PlaySkill(SkillSupportData data, BasicBattleComponent battleComponent)
    //{
    //    base.PlaySkill(data, battleComponent);
    //    transform.position = data.player.transform.position;
    //    SkillTask().Forget();
    //}

    //public override void AddState()
    //{
    //    base.AddState();
    //    absorbThrust = -SkillData.KnockBackThrust;
    //    CircleCollider2D circle = GetComponent<CircleCollider2D>();
    //    circle.radius = SkillData.RadiusRange;
    //}

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.CompareTag(EntityTag.Enemy.ToString()) == true)
    //    {
    //        Enemy enemy = col.GetComponent<Enemy>();
    //        enemy.followComponent.StopFollow();
    //        enemyList.Add(enemy);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.CompareTag(EntityTag.Enemy.ToString()) == true)
    //    {
    //        Enemy enemy = col.GetComponent<Enemy>();
    //        enemy.followComponent.ResetFollow();
    //        enemyList.Remove(enemy);
    //    }
    //}

    //void SetGravity()
    //{
    //    for (int i = 0; i < enemyList.Count; i++)
    //    {
    //        Vector3 dir = -(enemyList[i].transform.position - transform.position).normalized * absorbThrust;
    //        enemyList[i].forceComponent.AddForceUsingVec(dir, ForceMode2D.Impulse);
    //    }
    //}

    //public async UniTaskVoid SkillTask()
    //{
    //    effect.PlayEffect();
    //    BasicTask.NowRunning = true;

    //    int damageTic = 0;
    //    int maxDamageTic = (int)(SkillData.Duration / SkillData.UseTick);

    //    while (damageTic < maxDamageTic)
    //    {
    //        SetGravity();
    //        await UniTask.Delay(TimeSpan.FromSeconds(SkillData.UseTick), cancellationToken: BasicTask.source.Token);
    //        damageTic += 1;
    //    }

    //    BasicTask.NowRunning = false;
    //    effect.StopEffect();
    //    DisableObject();
    //}
}
