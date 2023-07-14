using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class FollowComponent : MonoBehaviour
{
    //Enemy enemy;
    //Player player;
    //public int count = 0;

    //bool nowHit = false;
    //public bool NowHit
    //{
    //    get { return nowHit; }
    //}

    //bool pauseFollow = false;
    //public bool PauseFollow
    //{
    //    get { return pauseFollow; }
    //}

    //public GameObject doNotClose;

    public DrawGizmo stopGizmo; // 따라오다가 멈추는 거리 기준
    public DrawGizmo attackGizmo; // 공격을 시작하는 거리 기준

    void OnDrawGizmos()
    {
        stopGizmo.DrawCircleGizmo(transform);
        attackGizmo.DrawCircleGizmo(transform);
    }

    public bool IsDistanceLower(Vector3 enemyPos, float minDistance)
    {
        float distanceBetween = Vector2.Distance(transform.position, enemyPos);
        if (distanceBetween <= minDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2 ReturnDirVec(Vector3 pos)
    {
        return (pos - transform.position).normalized;
    }

    //// Start is called before the first frame update
    //private void Start()
    //{
    //    enemy = GetComponent<Enemy>();
    //    player = PlayManager.Instance.player;
    //}

    //public void WaitFollow()
    //{
    //    if (BasicTask.NowRunning == true) return;
    //    WaitFollowTask().Forget();
    //}

    //public void StopFollow()
    //{
    //    pauseFollow = true;
    //    CancelWaitFollow();
    //    enemy.StopMove();
    //}

    //public void ResetFollow()
    //{
    //    pauseFollow = false;
    //}

    //void CancelWaitFollow()
    //{
    //    BasicTask.CancelTask();
    //    if (nowHit == true) nowHit = false;
    //}

    //public async UniTaskVoid WaitFollowTask()
    //{
    //    BasicTask.NowRunning = true;
    //    nowHit = true;

    //    doNotClose.SetActive(false);
    //    enemy.PlayerMode = ActionMode.Hit;

    //    enemy.StopMove();
    //    await UniTask.Delay(TimeSpan.FromSeconds(enemy.enemyData.StunTime), cancellationToken: BasicTask.source.Token);

    //    enemy.PlayerMode = ActionMode.Follow;
    //    doNotClose.SetActive(true);

    //    nowHit = false;
    //    BasicTask.NowRunning = false;
    //}

    //bool CanFollowDistance(Vector2 enemyPos)
    //{
    //    float distanceBetween = Vector2.Distance(transform.position, enemyPos);
    //    if (distanceBetween >= enemy.enemyData.FollowMinDistance)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //bool CanStopDistance(Vector2 enemyPos)
    //{
    //    float distanceBetween = Vector2.Distance(transform.position, enemyPos);
    //    if (distanceBetween <= enemy.enemyData.StopMinDistance)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //void CanUseSkillDistance(Vector2 enemyPos)
    //{
    //    float distanceBetween = Vector2.Distance(transform.position, enemyPos);
    //    if (distanceBetween <= enemy.enemyData.SkillMinDistance)
    //    {
    //        if (skillAction != null) skillAction();
    //    }
    //}



    //public void MoveToPlayer()
    //{
    //    if (nowHit == true || pauseFollow == true) return; // 루틴 돌아가는 동안, 사용 금지

    //    Vector2 dirVec = player.transform.position - enemy.transform.position;
    //    RotateUsingVelocity(dirVec.normalized);

    //    bool nowFollow = CanFollowDistance(player.transform.position);
    //    bool nowStop = CanStopDistance(player.transform.position);

    //    CanUseSkillDistance(player.transform.position);

    //    if (nowStop == true)
    //    {
    //        if (stopAction != null) stopAction();
    //        enemy.StopMove();
    //    }
    //    else if(nowFollow == true && nowStop == false)
    //    {
    //        enemy.rigid.velocity = (player.rigid.position - enemy.rigid.position).normalized * enemy.enemyData.Speed;
    //    }
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    ResetWhenDisable();
    //}

    //void ResetWhenDisable()
    //{
    //    if (nowHit == true) nowHit = false;
    //    if (pauseFollow == true) pauseFollow = false;
    //}

    //float CheckCanRotate(Vector2 vec)
    //{
    //    return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    //}

    //public void RotateUsingVelocity(Vector2 vec)
    //{
    //    enemy.rigid.MoveRotation(CheckCanRotate(vec));
    //}
}
