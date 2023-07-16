using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WhenUncountable", menuName = "Scriptable Object/SkillUseCountCondition/WhenUncountable", order = int.MaxValue)]
public class WhenUncountable : SkillUseCountCondition
{
    public override int Usage(int useCount)
    {
        return useCount;
    }
}
