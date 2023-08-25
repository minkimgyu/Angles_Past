using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "YellowPentagonSO", menuName = "Scriptable Object/EntitySO/YellowPentagonSO")]
public class YellowPentagonSO : BaseEntitySO
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

    public override Entity Create()
    {
        YellowPentagonEnemy enemy = ObjectPooler.SpawnFromPool<YellowPentagonEnemy>(name);

        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score,
            skillUseDistance, skillUseOffsetDistance, followDistance, followOffsetDistance, attackDelay);

        // 다음과 같이 초기화

        return enemy;
    }
}
