using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotationBallProjectile : BasicProjectile
{
    public override void DoUpdate()
    {
    }

    protected override void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        base.OnCollisionEnter2D(col);

        if(col.gameObject.tag == "Enemy")
        {
            NowFinish(); //  조건도 추가
            SoundManager.Instance.PlaySFX(transform.position, "SpawnBallDestroy", 0.25f);
        }
            
    }

    public override void OnEnd()
    {
        ObjectPooler.ReturnToTransform(transform);
        base.OnEnd();
    }
}