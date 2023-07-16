using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WhenCountUp", menuName = "Scriptable Object/SkillSynthesisCondition/WhenCountUp", order = int.MaxValue)]
public class WhenCountUp : SkillSynthesisCondition
{
    public override int Synthesis(int useCount, int maxUseCount)
    {
        return useCount + maxUseCount;
    }
}
