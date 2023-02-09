using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicReflectComponent : MonoBehaviour
{
    protected Entity entity;
    protected ForceComponent forceComponent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        forceComponent = GetComponent<ForceComponent>();
        entity.collisionEnterAction += ReflectEntity;
    }

    public virtual void ReflectEntity(Collision2D col)
    {

    }

    public void KnockBack(Vector2 dir)
    {
        forceComponent.CancelTask();
        forceComponent.AddForceUsingVec(dir);
    }

    private void OnDisable()
    {
        if (entity == null) return;
        entity.collisionEnterAction -= ReflectEntity;
    }
}
