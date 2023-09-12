using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class SlowEnemyComponent : UnitaskUtility
{
    //Enemy enemy;
    //CircleCollider2D slowRange;
    //public GameObject go;
    //public SpriteRenderer sr;

    //[SerializeField]
    //DrawGizmo slowRangeGizmo;

    //void OnDrawGizmos()
    //{
    //    slowRangeGizmo.DrawCircleGizmo(transform);
    //}

    //private void Start()
    //{
    //    enemy = GetComponentInParent<Enemy>();
    //    slowRange = GetComponent<CircleCollider2D>();
    //    //slowRange.radius = enemy.enemyData.SkillUseRange;

    //    sr = GetComponent<SpriteRenderer>();
    //}

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.CompareTag(EntityTag.PlayerTransform.ToString()) == true)
    //    {
    //        DatabaseManager.Instance.PlayerData.SpeedRatio -= 0.25f;
    //        FillEffect(0.6f);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.CompareTag(EntityTag.PlayerTransform.ToString()) == true)
    //    {
    //        DatabaseManager.Instance.PlayerData.SpeedRatio += 0.25f;
    //        FillEffect(0);
    //    }
    //}

    //void FillEffect(float targetAlpha)
    //{
    //    BasicTask.CancelTask();
    //    FillSprite(targetAlpha).Forget();
    //}

    //public async UniTaskVoid FillSprite(float targetAlpha)
    //{
    //    BasicTask.NowRunning = true;

    //    float fillRate = 0.01f;
    //    Color curColor = sr.color;
    //    while (Mathf.Abs(curColor.a - targetAlpha) > 0.001f) 
    //    {
    //        curColor.a = Mathf.Lerp(curColor.a, targetAlpha, Time.deltaTime);
    //        sr.color = curColor;
    //        await UniTask.Delay(TimeSpan.FromSeconds(fillRate), cancellationToken: BasicTask.source.Token);
    //    }

    //    BasicTask.NowRunning = false;
    //}
}
