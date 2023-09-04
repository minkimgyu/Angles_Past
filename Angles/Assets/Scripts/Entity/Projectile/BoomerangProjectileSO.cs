using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "BoomerangProjectileSO", menuName = "Scriptable Object/ProjectileSO/BoomerangProjectileSO")]
public class BoomerangProjectileSO : BasicProjectileSO
{
    [SerializeField]
    string projectileName;

    [SerializeField]
    float disableTime;

    [SerializeField]
    string[] skillNames;

    [SerializeField]
    EntityTag[] hitTargetTag;

    [SerializeField]
    float speed;

    public override BasicSpawnedObject Create()
    {
        BoomerangProjectile boomerang = ObjectPooler.SpawnFromPool<BoomerangProjectile>(projectileName);
        boomerang.Inintialize(disableTime, skillNames, hitTargetTag, speed);

        return boomerang;
    }
}
