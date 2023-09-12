using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BossTriangleSO", menuName = "Scriptable Object/EntitySO/BossTriangleSO")]
public class BossTriangleSO : BaseEntitySO
{
    [SerializeField]
    bool immortality;

    [SerializeField]
    BuffFloat hp;

    [SerializeField]
    BuffFloat speed;

    [SerializeField]
    BuffFloat stunTime;

    [SerializeField]
    BuffFloat weight;

    [SerializeField]
    BuffFloat mass;

    [SerializeField]
    BuffFloat drag;

    [SerializeField]
    string dieEffectName;

    [SerializeField]
    string[] skillNames;

    [SerializeField]
    BuffInt score;

    [SerializeField]
    BuffInt goldCount;

    [SerializeField]
    BuffFloat spawnPercentage;

    [SerializeField]
    BuffFloat skillUseDistance;

    [SerializeField]
    BuffFloat skillUseOffsetDistance;

    [SerializeField]
    BuffFloat followDistance;

    [SerializeField]
    BuffFloat followOffsetDistance;

    public override Entity Create()
    {
        BossTriangleEnemy enemy = ObjectPooler.SpawnFromPool<BossTriangleEnemy>(name);

        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage,
            skillUseDistance, skillUseOffsetDistance, followDistance, followOffsetDistance);

        // 다음과 같이 초기화

        return enemy;
    }
}
