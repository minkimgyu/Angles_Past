using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITag
{
    public EntityTag ReturnEntityTag();
}

public interface IBuffInvoker
{
    public void AddBuffToController(BuffData data);

    public void RemoveBuffToController(BuffData data);
}

public interface IHealth : ITag, IBuffInvoker
{
    public void UnderAttack(float healthPoint, Vector2 dir, float thrust);

    public void Heal(float healthPoint);

    public void Die();

    public void WhenUnderAttack(float healthPoint, Vector2 dir, float thrust);

    public void WhenHeal();
}