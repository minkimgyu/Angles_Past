using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BasicEffectPlayer : PositionDesignation
{
    protected float m_duration;

    public override void Init(Transform transform)
    {
        base.Init(transform);
        m_duration = -1;
    }

    public void Init(Transform transform, float duration)
    {
        base.Init(transform);
        m_duration = duration;
    }

    // lifeTime �� ���, ���� Const ������ ���� ����
    //  --> ���� ���� ����Ʈ�� ���Ǳ� ������ preDelay Ÿ�ӿ� �����Ű��
    public void Init(Transform transform, float duration, float scale)
    {
        base.Init(transform);
        m_duration = duration;
        ResetSize(scale);
    }

    public void Init(Transform transform, float duration, float[] lifeTime)
    {
        base.Init(transform);
        m_duration = duration;
        ResetLifeTime(lifeTime);
    }

    public void Init(Transform transform, float duration, float scale, float[] lifeTime)
    {
        base.Init(transform);
        m_duration = duration;
        ResetSize(scale);
        ResetLifeTime(lifeTime);
    }

    public void Init(Vector3 pos, float duration)
    {
        base.Init(pos);
        m_duration = duration;
    }

    public void Init(Vector3 pos, float duration, float scale)
    {
        base.Init(pos);
        m_duration = duration;
        ResetSize(scale);
    }

    public void Init(Vector3 pos, float duration, float[] lifeTime)
    {
        base.Init(pos);
        m_duration = duration;
        ResetLifeTime(lifeTime);
    }

    public void Init(Vector3 pos, float duration, float scale, float[] lifeTime)
    {
        base.Init(pos);
        m_duration = duration;
        ResetSize(scale);
        ResetLifeTime(lifeTime);
    }

    public virtual void Init(Vector3 posVec, float duration, List<Vector3> pos) { }

    protected virtual void ResetSize(float sizeMultiplier) { }
    protected virtual void ResetLifeTime(float[] lifeTime) { }

    public abstract void RotationEffect(float rotation);

    public abstract void PlayEffect();

    public abstract void StopEffect();

    protected virtual void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }
}