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
        BasicTask.WaitTime = 0.05f;
    }

    public void ReFillDashRatio()
    {
        if (BasicTask.NowRunning != false) return;

        FillDashRatio().Forget();
    }
    void FillDashIcon(float ratio) /// 0.66435
    {
        float perImage = 1 / DatabaseManager.Instance.PlayerData.MaxDashCount;
        bool nowFillComplete = false;


        for (int i = 1; i < dashImage.Length + 1; i++)
        {
            int indexOfImage = i - 1;


            if(nowFillComplete == true)
            {
                dashImage[indexOfImage].fillAmount = 0;
                continue;
            }

            float perImageRatio = perImage * i;

            if (ratio > perImageRatio)
            {
                dashImage[indexOfImage].fillAmount = 1;
            }
            else
            {
                int lastIndex = i - 1;
                if (lastIndex < 0) lastIndex = 0;

                float lastPerImageRatio = ratio - (perImage * (lastIndex));
                dashImage[indexOfImage].fillAmount = lastPerImageRatio * DatabaseManager.Instance.PlayerData.MaxDashCount;
                nowFillComplete = true;
            }
        }
    }

    private async UniTaskVoid FillDashRatio()
    {
        BasicTask.NowRunning = true;

        while (DatabaseManager.Instance.PlayerData.DashRatio < 1)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(BasicTask.WaitTime), cancellationToken: BasicTask.source.Token);
            DatabaseManager.Instance.PlayerData.DashRatio += 0.01f;

            FillDashIcon(DatabaseManager.Instance.PlayerData.DashRatio);
        }

        if (DatabaseManager.Instance.PlayerData.DashRatio != 1) DatabaseManager.Instance.PlayerData.DashRatio = 1;

        BasicTask.NowRunning = false;
    }
}
