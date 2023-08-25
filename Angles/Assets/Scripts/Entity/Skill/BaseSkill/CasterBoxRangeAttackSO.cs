using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CasterBoxRangeAttackSO", menuName = "Scriptable Object/SkillSO/CasterBoxRangeAttackSO")]
public class CasterBoxRangeAttackSO : BaseSkillSO
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
    Vector2[] boxRangePerTick;

    [SerializeField]
    float[] skillScalePerTicks;

    [SerializeField]
    Vector2[] offsetRangePerTick;

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
        return new CasterBoxRangeAttack(name, useConditionType, isFix, duration, tickCount, preDelay, boxRangePerTick, offsetRangePerTick, skillScalePerTicks, hitTarget, knockBackThrust, damage, effectDatas, soundDatas);
    }
}
