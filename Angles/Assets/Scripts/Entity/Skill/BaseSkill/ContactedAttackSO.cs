using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ContactedAttackSO", menuName = "Scriptable Object/SkillSO/ContactedAttackSO")]
public class ContactedAttackSO : BaseSkillSO
{
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
        return new ContactedAttack(name, hitTarget, knockBackThrust, damage, effectDatas, soundDatas);
    }
}
