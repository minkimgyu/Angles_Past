using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CasterCircleRangeAttackSO", menuName = "Scriptable Object/SkillSO/CasterCircleRangeAttackSO")]
public class CasterCircleRangeAttackSO : BaseSkillSO
{
    [SerializeField]
    BaseSkill.UseConditionType useConditionType;

    [SerializeField]
    bool isFix;

    [SerializeField]
    float duration;

    [SerializeField]
    int tickCount;

    [SerializeField]
    float preDelay;

    [SerializeField]
    float targetFindRange;

    [SerializeField]
    float[] skillScalePerTicks;

    [SerializeField]
    EntityTag[] hitTarget;

    [SerializeField]
    float knockBackThrust;

    [SerializeField]
    float damage;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new CasterCircleRangeAttack(name, useConditionType, isFix, duration, tickCount, preDelay, targetFindRange, skillScalePerTicks, hitTarget, knockBackThrust, damage, effectDatas, soundDatas);
    }
}
