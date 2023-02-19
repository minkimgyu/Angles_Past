using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect : BasicEffect
{


    private void OnTriggerEnter2D(Collider2D col)
    {
        print(col.gameObject.name); // ����Ʈ�� ���� - ��� ���� ����

        ObjectPooler.ReturnToTransform(transform);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
