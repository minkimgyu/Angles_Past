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

        PlayerData playerData = DatabaseManager.Instance.PlayerData;

        while (playerData.RushRatio < 1)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(WaitTIme), cancellationToken: source.Token);
            playerData.RushRatio += 0.01f;
            rushImage.fillAmount = playerData.RushRatio;
        }

        if (playerData.RushRatio > 1) playerData.RushRatio = 1;

        nowRunning = false;
    }

    public void EndRush()
    {
        DatabaseManager.Instance.PlayerData.StoredRushRatio = DatabaseManager.Instance.PlayerData.RushRatio;
        DatabaseManager.Instance.PlayerData.RushRatio = 0;
        rushImage.fillAmount = 0;
    }
}
