using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "OrangeSquareSO", menuName = "Scriptable Object/EntitySO/OrangeSquareSO")]
public class OrangeSquareSO : BaseEntitySO
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
    BuffFloat skillUseDistance;

    [SerializeField]
    BuffFloat skillUseOffsetDistance;

    [SerializeField]
    BuffFloat followDistance;

    [SerializeField]
    BuffFloat followOffsetDistance;

    [SerializeField]
    public string spawnEntityName;

    [SerializeField]
    public int spawnCount;

    [SerializeField]
    public SpawnCueEventSO spawnSO;

    public override Entity Create()
    {
        OrangeSquareEnemy enemy = ObjectPooler.SpawnFromPool<OrangeSquareEnemy>(name);

        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score,
            skillUseDistance, skillUseOffsetDistance, followDistance, followOffsetDistance, spawnEntityName,
            spawnCount, spawnSO);

        // 다음과 같이 초기화

        return enemy;
    }
}
