using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class ShootBulletSkill : MonoBehaviour
{
    //Transform entityTr;

    //public override void PlaySkill(Transform tr, BasicBattleComponent battleComponent)
    //{
    //    base.PlaySkill(tr, battleComponent);
    //    transform.position = tr.position;
    //    entityTr = tr;

    //    SkillTask().Forget();
    //}

    //void Shoot(int rotation)
    //{
    //    Vector3 direction = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
    //    Vector3 tempPos = entityTr.position + direction * 1.5f;

    //    GameObject go = ObjectPooler.SpawnFromPool("Bullet", tempPos, Quaternion.Euler(0,0, rotation));
    //    go.GetComponent<BasicBullet>().Fire(direction * SkillData.KnockBackThrust);
    //}

    //public async UniTaskVoid SkillTask()
    //{
    //    BasicTask.NowRunning = true;

    //    for (int i = 1; i <= 5; i++)
    //    {
    //        int rotation = 72 * i;
    //        Shoot(rotation);
    //        await UniTask.Delay(TimeSpan.FromSeconds(SkillData.PreDelay), cancellationToken: BasicTask.source.Token);
    //    }

    //    BasicTask.NowRunning = false;
    //}
}
