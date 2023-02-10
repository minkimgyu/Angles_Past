using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect : BasicEffect
{
    protected override void OnEnable()
    {
        PlayEffect();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print(col.gameObject.name); // 이펙트로 공격 - 방어 같이 진행
        ObjectPooler.ReturnToPool(gameObject, true);    // 한 객체에 한번만
        DisableObject();
    }


    protected override void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject, true);    // 한 객체에 한번만 
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면
    }
}
