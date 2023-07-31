using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotationBallProjectile : BasicProjectile
{
    public override void DoUpdate()
    {
    }

    protected override void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ
    {
        base.OnCollisionEnter2D(col);

        if(col.gameObject.tag == "Enemy")
        {
            NowFinish(); //  ���ǵ� �߰�
            SoundManager.Instance.PlaySFX(transform.position, "SpawnBallDestroy", 0.25f);
        }
            
    }

    public override void OnEnd()
    {
        ObjectPooler.ReturnToTransform(transform);
        base.OnEnd();
    }
}