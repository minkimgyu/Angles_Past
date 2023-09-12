using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CastBuffToPlayerSO", menuName = "Scriptable Object/SkillSO/CastBuffToPlayerSO")]
public class CastBuffToPlayerSO : BaseSkillSO
{
    [SerializeField]
    BaseSkill.UseConditionType useConditionType;

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
        EntityTag[] targets = { EntityTag.Player };
        return new CastBuffToPlayer(name, useConditionType, targets, nowApply, buffNames, effectDatas, soundDatas);
    }
}
