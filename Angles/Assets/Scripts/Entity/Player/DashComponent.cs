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
    private void Start()
    {
        player = GetComponent<Player>();
        PlayManager.Instance.actionJoy.actionComponent.dashAction += PlayDash;
    }

    public void PlayDash(Vector2 dir)
    {
        if (player.PlayerMode != ActionMode.Idle) return;

        DatabaseManager.Instance.PlayerData.SubtractRatio(); // 대쉬 사용한 만큼 빼준다.
        DashTask(dir).Forget();
    }

    private async UniTaskVoid DashTask(Vector2 dir)
    {
        BasicTask.NowRunning = true;
        player.PlayerMode = ActionMode.Dash;

        player.rigid.AddForce(dir * DatabaseManager.Instance.PlayerData.DashThrust, ForceMode2D.Impulse);
        await UniTask.Delay(TimeSpan.FromSeconds(DatabaseManager.Instance.PlayerData.DashTime), cancellationToken: BasicTask.source.Token);

        player.PlayerMode = ActionMode.Idle;
        BasicTask.NowRunning = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (PlayManager.Instance == null) return;

        PlayManager.Instance.actionJoy.actionComponent.dashAction -= PlayDash;
    }
}