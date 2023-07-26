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

    float goDuration;
    float returnDuration;
    float smoothness = 0.001f;


    Transform casterTr;

    CancellationTokenSource _source = new();

    Vector3 returnPoint;

    bool nowReturn = false;

    protected override void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ
    {
        base.OnCollisionEnter2D(col);
    }

    protected override void Awake()
    {
        base.Awake();
        Invoke("OnEnd", endTime);
    }

    Vector2 ReturnTruningPoint(Vector2 pos, Vector2 casterDir)
    {
        return pos + casterDir.normalized * 10;
    }

    public void ShootBoomerang(GameObject caster)
    {
        Vector2 tmpDir = caster.GetComponent<Rigidbody2D>().velocity;
        Vector2 pos = caster.transform.position;

        casterTr = caster.transform;
        returnPoint = ReturnTruningPoint(pos, tmpDir);

        MoveTask().Forget();
    }

    float ReturnDistanceOffset(Vector3 origin, Vector3 destination)
    {
        return Mathf.Abs((origin - destination).magnitude);
    }

    private async UniTaskVoid MoveTask()
    {
        while (ReturnDistanceOffset(transform.position, returnPoint) > 0.1f) // ���������� ����
        {
            transform.position = Vector2.MoveTowards(transform.position, returnPoint, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        transform.position = returnPoint;

        while (ReturnDistanceOffset(transform.position, casterTr.position) > 0.1f) // �ٽ� ���ƿ�
        {
            transform.position = Vector2.MoveTowards(transform.position, casterTr.position, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        while(true) // ��ġ�� �÷��̾ ������Ŵ
        { 
            transform.position = casterTr.position;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }
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
        _source.Cancel();
        casterTr = null;
        returnPoint = Vector3.zero;
        base.OnDisable();
    }
}
