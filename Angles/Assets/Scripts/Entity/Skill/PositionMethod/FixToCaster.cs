using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "FixToCaster", menuName = "Scriptable Object/PositionMethod/FixToCaster", order = int.MaxValue)]
public class FixToCaster : PositionMethod
{
    public override void DoUpdate(BasicSkill me)
    {
        if (me.PosTr == null) return;

        me.transform.position = me.PosTr.position; // 계속 고정시킴 // --> 돌아가는 기능을 넣어보자
    }

    public override void Init(Transform caster, BasicSkill me)
    {
        me.PosTr = caster;
    }
}
