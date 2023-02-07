using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class DashComponent : UnitaskUtility
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        PlayManager.Instance.actionJoy.actionComponent.dashAction += PlayDash;
    }

    public void PlayDash(Vector2 dir)
    {
        if (player.PlayerMode != PlayerMode.Idle) return;

        DatabaseManager.Instance.SubtractRatio(); // 대쉬 사용한 만큼 빼준다.
        DashTask(dir).Forget();
    }

    private async UniTaskVoid DashTask(Vector2 dir)
    {
        NowRunning = true;
        player.PlayerMode = PlayerMode.Dash;

        player.rigid.AddForce(dir * DatabaseManager.Instance.DashThrust, ForceMode2D.Impulse);
        await UniTask.Delay(TimeSpan.FromSeconds(DatabaseManager.Instance.DashTime), cancellationToken: source.Token);

        player.PlayerMode = PlayerMode.Idle;
        NowRunning = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayManager.Instance.actionJoy.actionComponent.dashAction -= PlayDash;
    }
}