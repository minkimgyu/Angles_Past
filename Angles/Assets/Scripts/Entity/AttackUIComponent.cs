using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class AttackUIComponent : MonoBehaviour
{
    RectTransform rush;
    public RectTransform Rush {
        get { return rush; }
    }


    Image rushImage;
    float rushRatio = 0;
    float rushPower = 5;

    UnitaskUtility fillTask = new UnitaskUtility();
    private void Start()
    {
        rushImage = rush.GetComponent<Image>();
        fillTask.cancelFn += EndRush;
    }

    public async UniTaskVoid FillAttackPower()
    {
        fillTask.NowRunning = true;
        EndRush();

        while (rushRatio < 1)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.01f), cancellationToken: fillTask.source.Token);
            rushRatio += 0.01f;
            rushImage.fillAmount = rushRatio;
        }

        if (rushRatio > 1) rushRatio = 1;

        fillTask.NowRunning = false;
    }

    public float ReturnRushStat()
    {
        fillTask.CancelTask();
        return rushPower * rushRatio;
    }

    public void EndRush()
    {
        rushRatio = 0;
        rushImage.fillAmount = 0;
    }

    private void OnDestroy()
    {
        fillTask.WhenDestroy();
    }

    private void OnDisable()
    {
        fillTask.WhenDisable();
    }

    private void OnEnable()
    {
        fillTask.WhenEnable();
    }
}
