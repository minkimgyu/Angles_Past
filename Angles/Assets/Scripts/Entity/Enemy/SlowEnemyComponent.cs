using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class SlowEnemyComponent : UnitaskUtility
{
    public GameObject go;
    public SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(EntityTag.Player.ToString()) == true)
        {
            DatabaseManager.Instance.PlayerData.SpeedRatio -= 0.25f;
            FillEffect(0.6f);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(EntityTag.Player.ToString()) == true)
        {
            DatabaseManager.Instance.PlayerData.SpeedRatio += 0.25f;
            FillEffect(0);
        }
    }

    void FillEffect(float targetAlpha)
    {
        CancelTask();
        FillSprite(targetAlpha).Forget();
    }

    public async UniTaskVoid FillSprite(float targetAlpha)
    {
        nowRunning = true;

        Debug.Log(targetAlpha);

        float fillRate = 0.01f;
        Color curColor = sr.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.001f) 
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, Time.deltaTime);
            sr.color = curColor;
            await UniTask.Delay(TimeSpan.FromSeconds(fillRate), cancellationToken: source.Token);
        }

        nowRunning = false;
    }
}
