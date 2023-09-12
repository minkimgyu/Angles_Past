using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ShootSpawnedObjectSO", menuName = "Scriptable Object/SkillSO/ShootSpawnedObjectSO")]
public class ShootSpawnedObjectSO : BaseSkillSO
{
    [SerializeField]
    BaseSkill.UseConditionType useConditionType;

    [SerializeField]
    string projectileName;

    [SerializeField]
    float duration;

    [SerializeField]
    int tickCount;

    [SerializeField]
    float preDelay;

    [SerializeField]
    float speed;

    [SerializeField]
    bool isNeedCaster;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new ShootSpawnedObject(name, useConditionType, projectileName, duration, tickCount, preDelay, speed, isNeedCaster, effectDatas, soundDatas);
    }
}
