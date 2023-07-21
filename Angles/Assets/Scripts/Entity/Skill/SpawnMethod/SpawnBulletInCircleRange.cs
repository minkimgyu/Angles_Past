using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnBulletInCircleRange", menuName = "Scriptable Object/SpawnMethod/SpawnBulletInCircleRange", order = int.MaxValue)]
public class SpawnBulletInCircleRange : SpawnMethod
{
    [SerializeField]
    int angleCount; // 360을 angleCount로 나누기

    [SerializeField]
    float distanceFromOrigin; // 원점으로 부터 거리

    [SerializeField]
    bool isClockwise; // 시계 방향으로 쏠건지 유무

    [SerializeField]
    float bulletThrust; // 총알 속도

    float rotationDir 
    { 
        get 
        { 
            if(!isClockwise)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        } 
    }

    public override void Execute(SpawnSupportData supportData)
    {
        float rotation = (360 / angleCount) * supportData.m_TickCount * rotationDir;

        Vector3 direction = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
        Vector3 tempPos = supportData.Me.transform.position + direction * distanceFromOrigin;

        BasicBullet projectile = ObjectPooler.SpawnFromPool<BasicBullet>(projectileName, tempPos, Quaternion.Euler(0, 0, rotation));
        projectile.Fire(direction, bulletThrust);
        supportData.Me.SpawnedObjects.Add(projectile);
    }
}
