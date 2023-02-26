using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class ActionUIComponent : UnitaskUtility
{
    Image rushImage;
    float rushRatio = 0;
    public float RushRatio
    {
        get
        {
            return rushRatio;
        }
    }

    private void Start()
    {
        rushImage = GetComponent<ActionJoystick>().rush.GetComponent<Image>();
        cancelFn += EndRush;
        WaitTIme = 0.01f;
    }

    public async UniTaskVoid FillAttackPower()
    {
        nowRunning = true;
        EndRush();

        while (rushRatio < 1)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(WaitTIme), cancellationToken: source.Token);
            rushRatio += 0.01f;
            rushImage.fillAmount = rushRatio;
        }

        if (rushRatio > 1) rushRatio = 1;

        nowRunning = false;
    }

    public void EndRush()
    {
        rushRatio = 0;
        rushImage.fillAmount = 0;
    }
}
