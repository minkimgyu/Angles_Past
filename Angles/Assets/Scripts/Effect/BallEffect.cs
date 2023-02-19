using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect : BasicEffect
{


    private void OnTriggerEnter2D(Collider2D col)
    {
        print(col.gameObject.name); // 이펙트로 공격 - 방어 같이 진행

        ObjectPooler.ReturnToTransform(transform);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
