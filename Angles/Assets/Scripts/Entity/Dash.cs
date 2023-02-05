using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class Dash : MonoBehaviour
{
    Player player;
    UnitaskUtility dashTask = new UnitaskUtility();

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        player.UpdateAction += SubUpdate;
    }

    void SubUpdate()
    {
        //if() 대쉬 조건문

        //if (PlayManager.Instance.attackJoy.DoubleClickCheck())
        //{

        //}
        //FillDashRatio().Forget();
    }

    public void PlayDash(Vector2 dir)
    {
        DashTask(dir).Forget();
    }

    private async UniTaskVoid DashTask(Vector2 dir)
    {
        dashTask.NowRunning = true;

        player.rigid.AddForce(dir, ForceMode2D.Impulse);

        await UniTask.Delay(TimeSpan.FromSeconds(0.01f), cancellationToken: dashTask.source.Token);

        dashTask.NowRunning = false;
    }
    private void OnDestroy()
    {
        dashTask.WhenDestroy();
    }

    private void OnDisable()
    {
        dashTask.WhenDisable();
    }

    private void OnEnable()
    {
        dashTask.WhenEnable();
    }
}
