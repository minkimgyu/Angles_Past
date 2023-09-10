using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "OrangeTriangleSO", menuName = "Scriptable Object/EntitySO/OrangeTriangleSO")]
public class OrangeTriangleSO : BaseEntitySO
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

    [SerializeField]
    string skillEffectName;

    [SerializeField]
    public string spawnEntityName;

    [SerializeField]
    public int spawnCount;

    [SerializeField]
    public SpawnCueEventSO spawnSO;

    public override Entity Create()
    {
        OrangeTriangleEnemy enemy = ObjectPooler.SpawnFromPool<OrangeTriangleEnemy>(name);

        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage,
            skillUseDistance, skillUseOffsetDistance, followDistance, followOffsetDistance, skillEffectName, 
            spawnEntityName, spawnCount, spawnSO);

        // 다음과 같이 초기화

        return enemy;
    }
}
