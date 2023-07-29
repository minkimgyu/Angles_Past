using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeTriangleEnemy : YellowTriangleEnemy
{
    public override void Die()
    {
        // ����� 4�� ����
        for (int i = 0; i < 4; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
            Entity yellowTr = ObjectPooler.SpawnFromPool<Entity>("YellowTriangle");
            yellowTr.InitData();
            yellowTr.transform.position = transform.position + offset;
        }
        base.Die();
    }
}
