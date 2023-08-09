using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class BoomerangProjectile : BasicProjectile
{
    [SerializeField]
    float endTime = 10;

    [SerializeField]
    float speed = 10;

    float smoothness = 0.001f;

    CancellationTokenSource _source = new();
    Vector3 returnPoint;


    protected override void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        //base.OnCollisionEnter2D(col);
        if(col.gameObject.tag == "Wall")
        {
            ReturnToPlayer();
        }
    }

    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        Invoke("NowFinish", endTime);
    }

    Vector2 ReturnTruningPoint(Vector2 pos, Vector2 casterDir, float thrust)
    {
        return pos + casterDir * thrust;
    }

    public override void Shoot(Vector2 dir, float thrust)
    {
        Vector2 pos = m_caster.transform.position;
        returnPoint = ReturnTruningPoint(pos, dir, thrust);

        MoveTask().Forget();
    }

    float ReturnDistanceOffset(Vector3 origin, Vector3 destination)
    {
        return Mathf.Abs((origin - destination).magnitude);
    }

    private async UniTaskVoid MoveTask()
    {
        while (ReturnDistanceOffset(transform.position, returnPoint) > 0.1f) // 포지션으로 날라감
        {
            transform.position = Vector2.MoveTowards(transform.position, returnPoint, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        transform.position = returnPoint;

        while (ReturnDistanceOffset(transform.position, m_caster.position) > 0.1f) // 다시 돌아옴
        {
            transform.position = Vector2.MoveTowards(transform.position, m_caster.position, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        while(true) // 위치를 플레이어에 고정시킴
        { 
            transform.position = m_caster.position;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }
    }

    private async UniTaskVoid ReturnTask()
    {
        while (ReturnDistanceOffset(transform.position, m_caster.position) > 0.1f) // 다시 돌아옴
        {
            transform.position = Vector2.MoveTowards(transform.position, m_caster.position, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        while (true) // 위치를 플레이어에 고정시킴
        {
            transform.position = m_caster.position;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }
    }

    void ReturnToPlayer()
    {
        _source.Cancel();
        _source = null;
        _source = new();

        ReturnTask().Forget();
    }

    public override void DoUpdate()
    {
    }

    private void OnDestroy()
    {
        _source.Cancel();
        _source.Dispose();
    }

    private void OnEnable()
    {
        if (_source != null)
            _source.Dispose();

        _source = new();
    }

    protected override void OnDisable()
    {
        CancelInvoke();
        _source.Cancel();
        m_caster = null;
        returnPoint = Vector3.zero;
        base.OnDisable();
    }
}
