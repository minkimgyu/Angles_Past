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

    protected override void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ
    {
        base.OnCollisionEnter2D(col);

        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Wall")
            NowFinish(); //  ���ǵ� �߰�
    }

    public void Fire(Vector2 dir, float thrust)
    {
        m_dashComponent.PlayDash(dir, thrust);
    }

    public override void DoUpdate()
    {
    }
}
