using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotationBallProjectile : BasicProjectile
{
    protected override void OnDisable()
    {
        if(m_skill != null) m_skill.SpawnedObjectDisable(this);
        base.OnDisable();
    }

    protected override void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        base.OnCollisionEnter2D(col);

        ObjectPooler.ReturnToTransform(transform);
        gameObject.SetActive(false);
    }
}