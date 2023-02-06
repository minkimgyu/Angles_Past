using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class DashUIComponent : UnitaskUtility
{
    public Image[] dashImage;

    private void Start()
    {
        WaitTIme = 0.1f;
    }

    public void ReFillDashRatio()
    {
        if (NowRunning != false) return;

        FillDashRatio().Forget();
    }
    void FillDashIcon(float ratio)
    {
        float perImage = 1 / DatabaseManager.Instance.MaxDashCount; // ex) 이미지가 3개인 경우 각각 0.333
        bool fillComplete = false;

        // ratio가 0.7일 경우, 2개 채우고 나머지 1개 0.03333만큼만 채우기

        for (int i = 1; i < DatabaseManager.Instance.MaxDashCount - 1; i++)
        {
            if (fillComplete == true)
            {
                dashImage[i].fillAmount = 0;
                continue;
            }


            // 0, 1, 2 --> 0.333333, 0.6666666, 0.999999
            if (ratio > perImage * i)
            {
                Debug.Log(perImage * i);
                Debug.Log(ratio);
                if (dashImage[i].fillAmount != 1) dashImage[i].fillAmount = 1;
            }
            else if (ratio < perImage * i)
            {
                Debug.Log(perImage * i);

                float lastFill = ratio - (perImage * i - 1); // 현 레이트 값에서 이전 필 레이트를 빼면 채워야 할 값이 나옴
                dashImage[i].fillAmount = lastFill * DatabaseManager.Instance.MaxDashCount;
                fillComplete = true;
            }
        }
    }

    private async UniTaskVoid FillDashRatio()
    {
        NowRunning = true;

        while (DatabaseManager.Instance.DashRatio < 1)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(WaitTIme), cancellationToken: source.Token);
            DatabaseManager.Instance.DashRatio += 0.01f;

            FillDashIcon(DatabaseManager.Instance.DashRatio);
        }

        if (DatabaseManager.Instance.DashRatio != 1) DatabaseManager.Instance.DashRatio = 1;

        NowRunning = false;
    }
}
