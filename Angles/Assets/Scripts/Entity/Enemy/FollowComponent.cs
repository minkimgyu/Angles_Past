using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class FollowComponent : UnitaskUtility
{
    Entity entity;
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
        entity = GetComponent<Entity>();
        entity.fixedUpdateAction += MoveToPlayer;

        player = PlayManager.Instance.player;
        entity.PlayerMode = ActionMode.Follow;
    }

    public void WaitFollow()
    {
        if (NowRunning == true) return;
        WaitFollowTask().Forget();
    }

    public void StopFollow()
    {
        CancelTask();
        entity.StopMove();
    }

    public async UniTaskVoid WaitFollowTask()
    {
        nowRunning = true;
        nowHit = true;

        doNotClose.SetActive(false);
        entity.PlayerMode = ActionMode.Hit;

        entity.rigid.velocity = Vector2.zero;
        await UniTask.Delay(TimeSpan.FromSeconds(DatabaseManager.Instance.WaitTime), cancellationToken: source.Token);

        entity.PlayerMode = ActionMode.Follow;
        doNotClose.SetActive(true);

        nowHit = false;
        nowRunning = false;
    }

    void CheckDistance(Vector2 enemyPos)
    {
        float distanceBetween = Vector2.Distance(transform.position, enemyPos);
        if (distanceBetween <= distance)
        {
            if (followAction != null) followAction();
        }
    }

    public void MoveToPlayer()
    {
        if (nowHit == true) return; // 루틴 돌아가는 동안, 사용 금지

        CheckDistance(player.transform.position);

        Vector2 dirVec = player.transform.position - entity.transform.position;
        Vector2 nextVec = dirVec.normalized * DatabaseManager.Instance.FollowSpeed * Time.fixedDeltaTime * ratio;
        entity.rigid.MovePosition(entity.rigid.position + nextVec);

        RotateUsingVelocity(dirVec.normalized);
    }

    float CheckCanRotate(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    public void RotateUsingVelocity(Vector2 vec)
    {
        entity.rigid.MoveRotation(CheckCanRotate(vec));
    }
}
