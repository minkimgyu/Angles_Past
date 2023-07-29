using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BasicEffectPlayer : MonoBehaviour
{
    protected Transform m_posTr;
    bool m_isFix;
    public bool IsFixed { set { m_isFix = value; } }
    protected float m_duration;

    public void Init(Transform tr, float duration)
    {
        m_posTr = tr;
        transform.position = m_posTr.position;
        m_duration = duration;
    }

    public void Init(Vector3 pos, float duration)
    {
        m_posTr = null;
        m_isFix = false;
        transform.position = pos;
        m_duration = duration;
    }

    public abstract void Init(Transform tr, float duration, List<Vector3> pos);

    protected virtual void Update()
    {
        if(m_isFix)
        {
            transform.position = m_posTr.position;
        }
    }

    public abstract void RotationEffect(float rotation);

    public abstract void PlayEffect();

    public abstract void StopEffect();

    protected virtual void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }

    protected virtual void DisableObject()
    {
        gameObject.SetActive(false);
        m_posTr = null;
        m_isFix = false;
    }
}