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
    BaseSkill.OverlapType overlapType;

    [SerializeField]
    bool canFinish = true;

    [SerializeField]
    int useCount;

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
    float[] skillScalePerTicks = new float[1] { 1 };

    [SerializeField]
    Vector2[] offsetRangePerTick = new Vector2[1] { Vector2.zero };

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
        return new CasterCircleRangeAttack(name, useConditionType, overlapType, canFinish, useCount, isFix, duration, tickCount, preDelay, targetFindRange, 
            skillScalePerTicks, offsetRangePerTick, hitTarget, knockBackThrust, damage, effectDatas, soundDatas);
    }
}
