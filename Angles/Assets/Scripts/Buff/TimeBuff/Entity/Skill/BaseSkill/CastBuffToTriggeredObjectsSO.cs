using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CastBuffToTriggeredObjectsSO", menuName = "Scriptable Object/SkillSO/CastBuffToTriggeredObjectsSO")]
public class CastBuffToTriggeredObjectsSO : BaseSkillSO
{
    [SerializeField]
    BaseSkill.UseConditionType useConditionType;

    [SerializeField]
    EntityTag[] targets;

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
        return new CastBuffToTriggeredObject(name, useConditionType, targets, nowApply, buffNames, effectDatas, soundDatas);
    }
}
