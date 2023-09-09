using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BossRushSO", menuName = "Scriptable Object/EntitySO/BossRushSO")]
public class BossRushSO : BaseEntitySO
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
    BuffFloat attackDelay;

    [SerializeField]
    BuffFloat attackThrust;

    public override Entity Create()
    {
        BossRushEnemy enemy = ObjectPooler.SpawnFromPool<BossRushEnemy>(name);

        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score,
            skillUseDistance, skillUseOffsetDistance, followDistance, followOffsetDistance, attackDelay, attackThrust);

        // 다음과 같이 초기화

        return enemy;
    }
}
