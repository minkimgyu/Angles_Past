using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class HealthEntity<T> : StateMachineEntity<T>, IHealth//, IEntityData<W>/*, IBuff<W>*/
{
    BuffComponent m_buffComponent;
    public BuffComponent BuffComponent { get { return m_buffComponent; } }

    Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody { get { return m_rigidbody; } }

    bool m_canHit = false;
    public bool CanHit { get { return m_canHit; } set { m_canHit = value; } }

    HealthEntityData m_healthData;

    public HealthEntityData HealthData
    {
        get
        {
            return m_healthData;
        }
        set
        {
            m_healthData = value;
            if (m_rigidbody == null) return;

            m_rigidbody.mass = m_healthData.Weight;
            m_rigidbody.drag = m_healthData.Drag;
        }
    }

    protected virtual void ShowDieEffect()
    {
        BasicEffectPlayer effectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(m_healthData.DieEffectName);
        if (effectPlayer == null) return;

        effectPlayer.Init(transform.position, 2f);
        effectPlayer.PlayEffect();
    }

    protected virtual void Awake()
    {
        m_buffComponent = GetComponent<BuffComponent>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }    

    public virtual void Heal(float healthPoint) { }

    public virtual void UnderAttack(float damage, Vector2 dir, float thrust)
    {
        if (m_canHit) return;

        if (m_healthData.Hp > 0)
        {
            WhenUnderAttack(damage, dir, thrust);
            m_healthData.Hp -= damage;
            if (m_healthData.Hp <= 0)
            {
                Die();
                m_healthData.Hp = 0;
            }
        }
    }

    public virtual void WhenUnderAttack(float damage, Vector2 dir, float thrust)
    {
        m_dicState[CurrentStateName].ReceiveUnderAttack(damage, dir, thrust);
    }

    public virtual void WhenHeal() { }

    public virtual void Die()
    {
        ShowDieEffect();
        gameObject.SetActive(false);
    }

    public HealthEntityData ReturnHealthEntityData()
    {
        return m_healthData;
    }

    public EntityTag ReturnEntityTag()
    {
        return inheritedTag;
    }

    protected override void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }

    public BuffComponent ReturnBuffComponent()
    {
        return m_buffComponent;
    }
}
