using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : BasicProjectile
{
    DashComponent m_dashComponent;

    protected override void Awake()
    {
        base.Awake();
        m_dashComponent = GetComponent<DashComponent>();
    }

    public void Fire(Vector2 dir, float thrust)
    {
        m_dashComponent.PlayDash(dir, thrust);
    }

    public override void DoUpdate()
    {
    }
}
