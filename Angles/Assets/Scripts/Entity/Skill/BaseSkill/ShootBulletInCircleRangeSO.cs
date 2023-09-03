using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ShootBulletInCircleRangeSO", menuName = "Scriptable Object/SkillSO/ShootBulletInCircleRangeSO")]
public class ShootBulletInCircleRangeSO : BaseSkillSO
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
    float speed;

    [SerializeField]
    bool isClockwise;

    [SerializeField]
    float distanceFromCaster;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new ShootBulletInCircleRange(name, useConditionType, projectileName, duration, tickCount, projectileCount, preDelay, speed, isClockwise, distanceFromCaster, effectDatas, soundDatas);
    }
}
