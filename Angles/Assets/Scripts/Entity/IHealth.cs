using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITag
{
    public EntityTag ReturnEntityTag();
}

public interface IHealth : ITag
{
    public void UnderAttack(float healthPoint, Vector2 dir, float thrust);

    public void Heal(float healthPoint);

    public void Die();

    public HealthEntityData ReturnHealthEntityData(); // ���� ���� �� �۵�

    public void WhenUnderAttack(float healthPoint, Vector2 dir, float thrust);

    public void WhenHeal();
}