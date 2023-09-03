using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class BoomerangProjectile : BasicSpawnedObject, IProjectile
{
    Transform m_caster;

    // �Ʒ� �� ������ ������ �ʱ�ȭ
    float speed;

    CancellationTokenSource _source = new();
    Vector3 returnPoint;
    float smoothness = 0.001f;

    public void Inintialize(float disableTime, string[] skillNames, string[] hitTargetTag, float speed)
    {
        Inintialize(disableTime, skillNames);
        this.hitTargetTag = hitTargetTag;
        this.speed = speed;
    }

    protected void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ --> Trigger�� ��ȯ��Ű��
    {
        if (IsTarget(col.gameObject) == false) return;

        ReturnToPlayer();
    }

    public override void ResetObject(Transform caster)
    {
        base.ResetObject(caster);
        m_caster = caster;
    }

    Vector2 ReturnTruningPoint(Vector2 pos, Vector2 casterDir, float thrust)
    {
        return pos + casterDir * thrust;
    }

    public void Shoot(Vector2 dir, float thrust)
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
        while (ReturnDistanceOffset(transform.position, returnPoint) > 0.1f) // ���������� ����
        {
            transform.position = Vector2.MoveTowards(transform.position, returnPoint, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        transform.position = returnPoint;

        while (ReturnDistanceOffset(transform.position, m_caster.position) > 0.1f) // �ٽ� ���ƿ�
        {
            transform.position = Vector2.MoveTowards(transform.position, m_caster.position, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        while(true) // ��ġ�� �÷��̾ ������Ŵ
        { 
            transform.position = m_caster.position;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }
    }

    private async UniTaskVoid ReturnTask()
    {
        while (ReturnDistanceOffset(transform.position, m_caster.position) > 0.1f) // �ٽ� ���ƿ�
        {
            transform.position = Vector2.MoveTowards(transform.position, m_caster.position, speed * Time.deltaTime);
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }

        while (true) // ��ġ�� �÷��̾ ������Ŵ
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
        m_caster = null;
        returnPoint = Vector3.zero;
        base.OnDisable();
    }
}
