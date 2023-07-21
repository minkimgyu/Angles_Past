using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnBulletInCircleRange", menuName = "Scriptable Object/SpawnMethod/SpawnBulletInCircleRange", order = int.MaxValue)]
public class SpawnBulletInCircleRange : SpawnMethod
{
    [SerializeField]
    int angleCount; // 360�� angleCount�� ������

    [SerializeField]
    float distanceFromOrigin; // �������� ���� �Ÿ�

    [SerializeField]
    bool isClockwise; // �ð� �������� ����� ����

    [SerializeField]
    float bulletThrust; // �Ѿ� �ӵ�

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
