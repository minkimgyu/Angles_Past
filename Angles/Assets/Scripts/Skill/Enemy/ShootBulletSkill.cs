using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class ShootBulletSkill : BasicSkill
{
    Transform entityTr;

    public override void PlayBasicSkill(Transform tr)
    {
        transform.position = tr.position;
        entityTr = tr;

        SkillTask().Forget();
    }

    void Shoot(int rotation)
    {
        Vector3 direction = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
        Vector3 tempPos = entityTr.position + direction * 1.5f;

        GameObject go = ObjectPooler.SpawnFromPool("Bullet", tempPos, Quaternion.Euler(0,0, rotation));
        go.GetComponent<BasicBullet>().Fire(direction * 1);
    }

    public async UniTaskVoid SkillTask()
    {
        for (int i = 1; i <= 5; i++)
        {
            int rotation = 72 * i;
            Shoot(rotation);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: source.Token);
        }
    }
}