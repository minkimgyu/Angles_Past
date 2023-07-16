using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CallWhenName", menuName = "Scriptable Object/SkillCallCondition/CallWhenName", order = int.MaxValue)]
public class CallWhenName : SkillCallCondition
{
    public override bool Check(SkillUseType useType)
    {
       return false;
    }

    public override bool Check(string myName, string callName)
    {
        return myName == callName;
    }
}
