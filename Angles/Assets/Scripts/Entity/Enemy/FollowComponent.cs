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
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.fixedUpdateAction += MoveToPlayer;

        player = PlayManager.Instance.player;
        enemy.PlayerMode = ActionMode.Follow;
    }

    public void WaitFollow()
    {
        if (BasicTask.NowRunning == true) return;
        WaitFollowTask().Forget();
    }

    public void StopFollow()
    {
        BasicTask.CancelTask();
        enemy.StopMove();
    }

    public async UniTaskVoid WaitFollowTask()
    {
        BasicTask.NowRunning = true;
        nowHit = true;

        doNotClose.SetActive(false);
        enemy.PlayerMode = ActionMode.Hit;

        enemy.rigid.velocity = Vector2.zero;
        await UniTask.Delay(TimeSpan.FromSeconds(enemy.enemyData.StunTime), cancellationToken: BasicTask.source.Token);

        enemy.PlayerMode = ActionMode.Follow;
        doNotClose.SetActive(true);

        nowHit = false;
        BasicTask.NowRunning = false;
    }

    bool CanFollowDistance(Vector2 enemyPos)
    {
        float distanceBetween = Vector2.Distance(transform.position, enemyPos);
        if (distanceBetween >= enemy.enemyData.FollowMinDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CanStopDistance(Vector2 enemyPos)
    {
        float distanceBetween = Vector2.Distance(transform.position, enemyPos);
        if (distanceBetween <= enemy.enemyData.StopMinDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //float startRatio = 0;
    //float endRatio = 1;
    //float ratio = 1;
    //float speed = 1;
    //float up = 0.01f;
    //bool storedFollow = true;

    //public async UniTaskVoid LerpSpeed()
    //{
    //    tasks["LerpSpeed"].NowRunning = true;
    //    ratio = startRatio;

    //    while (Mathf.Abs(endRatio - ratio) >= 0.01f)
    //    {
    //        await UniTask.Delay(TimeSpan.FromSeconds(tasks["LerpSpeed"].WaitTime), cancellationToken: tasks["LerpSpeed"].source.Token);
    //        ratio = Mathf.Lerp(ratio, endRatio, Time.fixedDeltaTime * speed);
    //        print(ratio);
    //    }

    //    tasks["LerpSpeed"].NowRunning = false;
    //}

    public void MoveToPlayer()
    {
        if (nowHit == true) return; // 루틴 돌아가는 동안, 사용 금지

        Vector2 dirVec = player.transform.position - enemy.transform.position;
        RotateUsingVelocity(dirVec.normalized);

        bool nowFollow = CanFollowDistance(player.transform.position);
        bool nowStop = CanStopDistance(player.transform.position);

        if (nowStop == true)
        {
            enemy.rigid.velocity = Vector2.zero;
            print("Stop");
        }
        else if(nowFollow == true && nowStop == false)
        {
            if (followAction != null) followAction();
            enemy.rigid.velocity = (player.rigid.position - enemy.rigid.position).normalized * enemy.enemyData.Speed;
            print("Follow");
        }

        //print(nowFollow + "&" + storedFollow);

        //if (nowFollow == true && storedFollow == false)
        //{
        //    if(tasks["LerpSpeed"].NowRunning == false) LerpSpeed().Forget();
        //}
        //else if (nowFollow == false) 
        //{
        //    storedFollow = nowFollow;
        //    enemy.rigid.velocity = Vector2.zero; return; 
        //} // 정지

        //storedFollow = nowFollow;


        //if (nowFollow == false)
        //{
        //    //if(enemy.rigid.velocity != Vector2.zero)
        //    //    enemy.rigid.velocity = Vector2.zero;
        //}
        //else
        //{
        //    Vector2 nextVec = (enemy.rigid.position - player.rigid.position).normalized * enemy.enemyData.Speed;
        //    enemy.rigid.velocity = nextVec;
        //}

        //Vector2 nextVec = dirVec.normalized * enemy.enemyData.Speed * ratio;
        //enemy.rigid.velocity = nextVec;
        //enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
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
