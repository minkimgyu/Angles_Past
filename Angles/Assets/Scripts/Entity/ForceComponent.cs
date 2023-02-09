using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceComponent : UnitaskUtility
{
    protected Entity entity;

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
    }

    public virtual void AddForceUsingVec(Vector2 dir)
    {
        entity.rigid.AddForce(dir, ForceMode2D.Impulse);
    }

    protected virtual void StopEntity()
    {
        entity.rigid.velocity = Vector2.zero;
    }
}
