using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeSquareEnemy : YellowSquareEnemy
{
    public override void Die()
    {
        // ����� 4�� ����
        //for (int i = 0; i < 4; i++)
        //{
        //    Vector3 offset = new Vector3(Random.Range(1, 6), Random.Range(1, 6), 0);
        //    YellowSquareEnemy yellow = ObjectPooler.SpawnFromPool<YellowSquareEnemy>("YellowRectangle");
        //    yellow.InitData();
        //    yellow.transform.position = transform.position + offset;
        //}

        // �����ʿ��� �����ϰԲ� ������ֱ� --> SO �̿�

        base.Die();
    }
}
