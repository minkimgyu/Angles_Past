using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void UnderAttack(HealthEntityData data, float healthPoint, Vector2 dir, float thrust);

    public void Heal(HealthEntityData data, float healthPoint);

    public void Die();

    public HealthEntityData ReturnHealthEntityData();

    public void WhenUnderAttack();

    public void WhenHeal();
    public EntityTag ReturnEntityTag();
}