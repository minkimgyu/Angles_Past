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
    }

    public void KnockBack(Vector2 dir1)
    {
        forceComponent.CancelTask();
        forceComponent.AddForceUsingVec(dir1);
    }
}
