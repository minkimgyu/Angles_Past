using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PositionToCaster", menuName = "Scriptable Object/PositionMethod/PositionToCaster", order = int.MaxValue)]
public class PositionToCaster : PositionMethod
{
    public override void DoUpdate(BasicSkill me)
    {
        
    }

    public override void Init(Transform caster, BasicSkill me)
    {
        me.transform.position = caster.transform.position; // 한번만 움직임
    }
}
