using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GravitationalFieldSO", menuName = "Scriptable Object/ProjectileSO/GravitationalFieldSO")]
public class GravitationalFieldSO : BasicProjectileSO
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
    EntityTag[] disableTargetTag;

    [SerializeField]
    float absorbThrust;

    public override BasicSpawnedObject Create()
    {
        GravityBallProjectile gravitationalField = ObjectPooler.SpawnFromPool<GravityBallProjectile>(projectileName);
        gravitationalField.Inintialize(disableTime, skillNames, hitTargetTag, disableTargetTag, absorbThrust);

        return gravitationalField;
    }
}
