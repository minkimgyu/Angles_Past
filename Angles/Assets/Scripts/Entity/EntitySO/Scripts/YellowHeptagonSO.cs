using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "YellowHeptagonSO", menuName = "Scriptable Object/EntitySO/YellowHeptagonSO")]
public class YellowHeptagonSO : BaseEntitySO
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

    public override Entity Create()
    {
        BaseReflectEnemy enemy = ObjectPooler.SpawnFromPool<BaseReflectEnemy>(name);
        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score);  // 다음과 같이 초기화

        return enemy;
    }
}
