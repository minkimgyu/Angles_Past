using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CastBuffToCasterSO", menuName = "Scriptable Object/SkillSO/CastBuffToCasterSO")]
public class CastBuffToCasterSO : BaseSkillSO
{
    [SerializeField]
    BaseSkill.UseConditionType useConditionType;

    [SerializeField]
    EntityTag[] hitTarget;

    [SerializeField]
    bool nowApply;

    [SerializeField]
    string[] buffNames;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new CastBuffToCaster(name, useConditionType, hitTarget, nowApply, buffNames, effectDatas, soundDatas);
    }
}
