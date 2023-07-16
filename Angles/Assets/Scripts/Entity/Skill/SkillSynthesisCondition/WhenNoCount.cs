using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WhenNoCount", menuName = "Scriptable Object/SkillSynthesisCondition/WhenNoCount", order = int.MaxValue)]
public class WhenNoCount : SkillSynthesisCondition
{
    public override int Synthesis(int useCount, int maxUseCount)
    {
        return useCount;
    }
}
