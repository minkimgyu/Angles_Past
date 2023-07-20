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
        isFinished = true; //  ���ǵ� �߰�
    }

    public override void OnEnd()
    {
        ObjectPooler.ReturnToTransform(transform);
        base.OnEnd();
    }
}