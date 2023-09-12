using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : ContactableObject, IProjectile
{
    public DashComponent DashComponent { get; set; }

    protected override void Awake()
    {
        base.Awake();
        DashComponent = GetComponent<DashComponent>();
    }

    public override void ResetObject(Vector3 pos, float rotation)
    {
        base.ResetObject(pos, rotation);

        transform.position = pos;
        transform.rotation = Quaternion.Euler(0, 0, rotation);

    }

    public void Shoot(Vector2 dir, float thrust)
    {
        DashComponent.PlayDash(dir, thrust);
    }

    public void Inintialize(float disableTime, string[] skillNames, EntityTag[] hitTargetTag)
    {
        Inintialize(disableTime, skillNames);
        this.hitTargetTag = hitTargetTag;
    }

    protected override void OnDisable()
    {
        DashComponent.CancelDash();
        base.OnDisable();
    }
}
