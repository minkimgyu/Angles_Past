using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BasicBulletSO", menuName = "Scriptable Object/ProjectileSO/BasicBulletSO")]
public class BasicBulletSO : BasicProjectileSO
{
    [SerializeField]
    string projectileName;

    [SerializeField]
    string[] hitTargetTag;

    [SerializeField]
    string[] skillNames;

    [SerializeField]
    float disableTime;

    public override BasicSpawnedObject Create()
    {
        BasicBullet bullet = ObjectPooler.SpawnFromPool<BasicBullet>(projectileName);
        bullet.Inintialize(disableTime, skillNames, hitTargetTag);

        return bullet;
    }
}
