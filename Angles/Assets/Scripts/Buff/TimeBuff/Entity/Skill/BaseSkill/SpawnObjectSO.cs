using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnObjectSO", menuName = "Scriptable Object/SkillSO/SpawnObjectSO")]
public class SpawnObjectSO : BaseSkillSO
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
    bool isNeedCaster;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new SpawnObject(name, useConditionType, projectileName, duration, tickCount, preDelay, isNeedCaster, effectDatas, soundDatas);
    }
}
