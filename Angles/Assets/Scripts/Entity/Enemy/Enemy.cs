using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : Entity
{
    Player player;
    Rigidbody2D playerRigid;
    bool nowStop = false;
    float maxFollowDistance = 3f;

    CancellationTokenSource source = new();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GameObject playerGo = GameObject.FindWithTag("Player");

        player = playerGo.GetComponent<Player>();
        playerRigid = playerGo.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.collider.CompareTag("Wall"))
        //{
        //    Debug.Log("OnCollisionEnter2D");
        //}


        if(col.collider.CompareTag("Player") == true && player.PlayerMode == PlayerMode.Attack)
        {
            ExitMoveStopTask();

            nowStop = true;
            MoveStop().Forget();
            //MoveBack(col);
            gameObject.SetActive(false);
        }

        if (col.collider.CompareTag("Player") == true && player.PlayerMode == PlayerMode.Attack)
        {
            ExitMoveStopTask();

            nowStop = true;
            MoveStop().Forget();
            //MoveBack(col);
            gameObject.SetActive(false);
        }
    }

    void ExitMoveStopTask()
    {
       if (nowStop == true)
       {
            source.Cancel();
       }
    }

    private async UniTaskVoid MoveStop()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken:source.Token);
        nowStop = false;
    }

    public void MoveBack(Collision2D col)
    {
        var speed = 5;
        rigid.velocity = col.contacts[0].normal * speed;
        Debug.Log(rigid.velocity);
    }

    public void MoveToPlayer()
    {
        if (nowStop == true) return;

        float distance = Vector2.Distance(playerRigid.position, rigid.position);
        if (distance < maxFollowDistance)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

        Vector2 dirVec = playerRigid.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void OnDestroy()
    {
        source.Cancel();
        source.Dispose();
    }

    private void OnDisable()
    {
        source.Cancel();
        ObjectPooler.ReturnToPool(gameObject);
    }

    private void OnEnable()
    {
        if (source != null) source.Dispose();
        source = new CancellationTokenSource();
    }
}
