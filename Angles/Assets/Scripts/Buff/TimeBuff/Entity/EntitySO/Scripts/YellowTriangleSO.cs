using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "YellowTriangleSO", menuName = "Scriptable Object/EntitySO/YellowTriangleSO")]
public class YellowTriangleSO : BaseEntitySO
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

    public override Entity Create()
    {
        // storageSO --> ���⼭ �÷��̾ �Ѱ��ش�.

        YellowTriangleEnemy enemy = ObjectPooler.SpawnFromPool<YellowTriangleEnemy>(name);

        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage,
            skillUseDistance, skillUseOffsetDistance, followDistance, followOffsetDistance, skillEffectName);

        // ������ ���� �ʱ�ȭ

        return enemy;
    }
}
