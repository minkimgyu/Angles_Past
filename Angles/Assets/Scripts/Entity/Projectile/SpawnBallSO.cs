using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnBallSO", menuName = "Scriptable Object/ProjectileSO/SpawnBallSO")]
public class SpawnBallSO : BasicProjectileSO
{
    [SerializeField]
    string projectileName;

    [SerializeField]
    float disableTime;

    [SerializeField]
    float speed;

    [SerializeField]
    float distanceFromCaster;

    [SerializeField]
    string[] skillNames;

    [SerializeField]
    EntityTag[] hitTargetTag;

    public override BasicSpawnedObject Create()
    {
        RotationBallProjectile rotationBall = ObjectPooler.SpawnFromPool<RotationBallProjectile>(projectileName);
        rotationBall.Inintialize(disableTime, skillNames, hitTargetTag, speed);

        return rotationBall;
    }
}
