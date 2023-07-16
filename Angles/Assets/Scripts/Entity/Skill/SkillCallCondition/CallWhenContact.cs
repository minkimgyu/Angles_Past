using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CallWhenContact", menuName = "Scriptable Object/SkillCallCondition/CallWhenContact", order = int.MaxValue)]
public class CallWhenContact : SkillCallCondition
{
    public override bool Check(SkillUseType useType)
    {
        if (useType == SkillUseType.Contact) return true;
        else return false;
    }

    public override bool Check(string myName, string callName)
    {
        return false;
    }
}
