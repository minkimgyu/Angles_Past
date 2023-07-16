using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WhenCountable", menuName = "Scriptable Object/SkillUseCountCondition/WhenCountable", order = int.MaxValue)]
public class WhenCountable : SkillUseCountCondition
{
    public override int Usage(int useCount)
    {
        return useCount -= 1;
    }
}
