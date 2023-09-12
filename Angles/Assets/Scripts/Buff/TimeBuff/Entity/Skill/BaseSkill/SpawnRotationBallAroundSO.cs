using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnRotationBallAroundSO", menuName = "Scriptable Object/SkillSO/SpawnRotationBallAroundSO")]
public class SpawnRotationBallAroundSO : BaseSkillSO
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
    int projectileCount;

    [SerializeField]
    float preDelay;

    [SerializeField]
    float distanceFromCaster;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new SpawnRotationBallAround(name, useConditionType, projectileName, duration, tickCount, projectileCount, preDelay, distanceFromCaster, effectDatas, soundDatas);
    }
}
