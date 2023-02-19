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
    public bool once = false;
    public bool nowHit = false;

    public List<GameObject> closeEnemy = new List<GameObject>();

    public GameObject doNotClose;

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

    public async UniTaskVoid WaitFollowTask()
    {
        NowRunning = true;
        doNotClose.SetActive(false);
        entity.PlayerMode = ActionMode.Hit;

        entity.rigid.velocity = Vector2.zero;
        await UniTask.Delay(TimeSpan.FromSeconds(DatabaseManager.Instance.WaitTime), cancellationToken: source.Token);

        entity.PlayerMode = ActionMode.Follow;
        nowHit = false;
        once = false;
        doNotClose.SetActive(true);
        NowRunning = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            closeEnemy.Add(col.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            closeEnemy.Remove(col.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            FollowComponent followComponent = col.gameObject.GetComponent<FollowComponent>();

            if (NowRunning == true && followComponent.nowHit == false && followComponent.once == false)
            {
                Vector2 backDir = col.transform.position - transform.position;

                followComponent.WaitFollow();
                followComponent.once = true;

                if (count == 0)
                {
                    col.gameObject.GetComponent<BasicReflectComponent>().KnockBack(backDir);
                }
                else
                {
                    col.gameObject.GetComponent<BasicReflectComponent>().KnockBack(backDir * count);
                }

            }
        }
    }

    public void MoveToPlayer()
    {
        if (NowRunning == true)
        {
            return; // 루틴 돌아가는 동안, 사용 금지
        }

        Vector2 dirVec = player.transform.position - entity.transform.position;
        Vector2 nextVec = dirVec.normalized * DatabaseManager.Instance.FollowSpeed * Time.fixedDeltaTime * ratio;
        entity.rigid.MovePosition(entity.rigid.position + nextVec);
    }
}
