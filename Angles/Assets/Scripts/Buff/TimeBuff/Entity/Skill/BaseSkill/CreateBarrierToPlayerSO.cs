using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CreateBarrierToPlayerSO", menuName = "Scriptable Object/SkillSO/CreateBarrierToPlayerSO")]
public class CreateBarrierToPlayerSO : BaseSkillSO
{
    [SerializeField]
    BaseSkill.UseConditionType useConditionType;

    [SerializeField]
    float duration;

    [SerializeField]
    int tickCount;

    [SerializeField]
    float preDelay;

    [SerializeField]
    EffectConditionEffectDataDictionary effectDatas;

    [SerializeField]
    EffectConditionSoundDataDictionary soundDatas;

    public override BaseSkill Create()
    {
        return new CreateBarrierToPlayer(name, useConditionType, duration, tickCount, preDelay, effectDatas, soundDatas);
    }
}
