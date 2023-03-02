using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceComponent : UnitaskUtility
{
    protected Entity entity;
    protected override void Awake()
    {
        base.Awake();
        entity = GetComponent<Entity>();
    }

    public void PlayKnockBack(Vector2 dir)
    {
        BasicTask.CancelTask();
        AddForceUsingVec(dir);
    }

    public virtual void AddForceUsingVec(Vector2 dir, ForceMode2D forceMode = ForceMode2D.Impulse)
    {
        entity.rigid.AddForce(dir, forceMode);
    }

    protected virtual void StopEntity()
    {
        entity.rigid.velocity = Vector2.zero;
    }
}
