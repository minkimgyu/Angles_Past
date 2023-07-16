using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CallWhenGet", menuName = "Scriptable Object/SkillCallCondition/CallWhenGet", order = int.MaxValue)]
public class CallWhenGet : SkillCallCondition
{
    public override bool Check(SkillUseType useType)
    {
        if (useType == SkillUseType.Get) return true;
        else return false;
    }

    public override bool Check(string myName, string callName)
    {
        return false;
    }
}
