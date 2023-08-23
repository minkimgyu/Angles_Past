using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeTriangleEnemy : YellowTriangleEnemy
{
    public override void Die()
    {
        // 노란색 4개 스폰
        //for (int i = 0; i < 4; i++)
        //{
        //    Vector3 offset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
        //    Entity yellowTr = ObjectPooler.SpawnFromPool<Entity>("YellowTriangle");
        //    yellowTr.transform.position = transform.position + offset;
        //}

        // 스포너에서 스폰하게끔 만들어주기 --> SO 이용

        base.Die();
    }
}
