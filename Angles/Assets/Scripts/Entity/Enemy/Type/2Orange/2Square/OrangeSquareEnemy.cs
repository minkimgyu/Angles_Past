using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeSquareEnemy : YellowSquareEnemy
{
    public override void Die()
    {
        // 노란색 4개 스폰
        for (int i = 0; i < 4; i++)
        {
            Vector3 offset = new Vector3(Random.Range(1, 6), Random.Range(1, 6), 0);
            Transform yellowTr = ObjectPooler.SpawnFromPool<Transform>("YellowRectangle");
            yellowTr.position = transform.position + offset;
        }
        base.Die();
    }
}
