using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "FixToCasterAndRotation", menuName = "Scriptable Object/PositionMethod/FixToCasterAndRotation", order = int.MaxValue)]
public class FixToCasterAndRotation : PositionMethod
{
    public override void DoUpdate(BasicSkill me)
    {
        if (me.PosTr == null || me.Data == null) return;

        me.transform.position = me.PosTr.position; // 계속 고정시킴
        me.transform.RotateAround(me.transform.position, Vector3.forward, Time.deltaTime * me.Data.SpawnObjectSpeed);
    }

    public override void Init(Transform caster, BasicSkill me)
    {
        me.PosTr = caster;
    }
}
