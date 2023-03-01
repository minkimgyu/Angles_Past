using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class FollowComponent : UnitaskUtility
{
    Enemy enemy;
    Player player;
    public float ratio = 1;
    public int count = 0;

    bool nowHit = false;
    public bool NowHit
    {
        get { return nowHit; }
    }

    public float distance = 3f;
    public Action followAction;

    public GameObject doNotClose;

    public DrawGizmo stopGizmo; // 따라오다가 멈추는 거리 기준
    public DrawGizmo attackGizmo; // 공격을 시작하는 거리 기준

    void OnDrawGizmos()
    {
        stopGizmo.DrawCircleGizmo(transform);
        attackGizmo.DrawCircleGizmo(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.fixedUpdateAction += MoveToPlayer;

        player = PlayManager.Instance.player;
        enemy.PlayerMode = ActionMode.Follow;
    }

    public void WaitFollow()
    {
        if (NowRunning == true) return;
        WaitFollowTask().Forget();
    }

    public void StopFollow()
    {
        CancelTask();
        enemy.StopMove();
    }

    public async UniTaskVoid WaitFollowTask()
    {
        nowRunning = true;
        nowHit = true;

        doNotClose.SetActive(false);
        enemy.PlayerMode = ActionMode.Hit;

        enemy.rigid.velocity = Vector2.zero;
        await UniTask.Delay(TimeSpan.FromSeconds(enemy.enemyData.StunTime), cancellationToken: source.Token);

        enemy.PlayerMode = ActionMode.Follow;
        doNotClose.SetActive(true);

        nowHit = false;
        nowRunning = false;
    }

    bool CanFollowDistance(Vector2 enemyPos)
    {
        float distanceBetween = Vector2.Distance(transform.position, enemyPos);
        if (distanceBetween <= enemy.enemyData.FollowMinDistance)
        {
            if (followAction != null) followAction();
            return false;
        }
        else
        {
            return true;
        }
    }

    public void MoveToPlayer()
    {
        if (nowHit == true) return; // 루틴 돌아가는 동안, 사용 금지

        Vector2 dirVec = player.transform.position - enemy.transform.position;
        RotateUsingVelocity(dirVec.normalized);

        bool canFollow = CanFollowDistance(player.transform.position);
        if (canFollow == false)
        {
            enemy.rigid.MovePosition(enemy.rigid.position);
            return;
        }

        Vector2 nextVec = dirVec.normalized * enemy.enemyData.Speed * Time.fixedDeltaTime * ratio;
        enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
    }

    float CheckCanRotate(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    public void RotateUsingVelocity(Vector2 vec)
    {
        enemy.rigid.MoveRotation(CheckCanRotate(vec));
    }
}
