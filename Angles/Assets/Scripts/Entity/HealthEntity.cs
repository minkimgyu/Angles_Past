using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class HealthEntity<T> : StateMachineEntity<T>, IHealth
{
    public Action<float, Vector2, float> UnderAttackAction;

    public virtual void Heal(HealthEntityData data, float healthPoint)
    {
        if (data.Hp > 0)
        {
            WhenUnderAttack();
            data.Hp -= healthPoint;
            if (data.Hp <= 0)
            {
                Die();
                data.Hp = 0;
            }
        }

    }

    public virtual void UnderAttack(HealthEntityData data, float healthPoint, Vector2 dir, float thrust)
    {
        if (data.Hp > 0)
        {
            WhenUnderAttack();
            data.Hp -= healthPoint;
            if (data.Hp <= 0)
            {
                Die();
                data.Hp = 0;
            }
        }
    }

    public abstract void WhenUnderAttack();

    public abstract void WhenHeal();

    public abstract void Die();

    public abstract HealthEntityData ReturnHealthEntityData();

    public EntityTag ReturnEntityTag()
    {
        return inheritedTag;
    }
}
