using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ShootLaserToRandomDirectionSO", menuName = "Scriptable Object/SkillSO/ShootLaserToRandomDirectionSO")]
public class ShootLaserToRandomDirectionSO : BaseSkillSO
{
    [SerializeField]
    BaseSkill.UseConditionType useConditionType;

    [SerializeField]
    BaseSkill.OverlapType overlapType;

    [SerializeField]
    bool canFinish;

    [SerializeField]
    int useCount;

    [SerializeField]
    float duration;

    [SerializeField]
    int tickCount;

    [SerializeField]
    float preDelay;

    [SerializeField]
    float[] rangePerTicks;

    [SerializeField]
    EntityTag[] hitTarget;

    [SerializeField]
    float knockBackThrust;

    [SerializeField]
    float damage;

    [SerializeField]
    float laserMaxDistance;

    [SerializeField]
    EntityTag[] blockedTag;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new ShootLaserToRandomDirection(name, useConditionType, overlapType, canFinish, useCount, duration, tickCount, preDelay, rangePerTicks, hitTarget, knockBackThrust, damage, laserMaxDistance, blockedTag, effectDatas, soundDatas);
    }
}
