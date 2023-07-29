using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CastGhost", menuName = "Scriptable Object/DamageMethod/CastGhost", order = int.MaxValue)]
public class CastGhost : DamageMethod
{
    public override void Execute(DamageSupportData supportData)
    {
        supportData.Caster.TryGetComponent(out BuffComponent buffComponent);
        BuffComponent buff = buffComponent;

        BuffData data = DatabaseManager.Instance.UtilizationDB.ReturnBuffData("SpeedBuff");
        buff.AddBuff(data);
    }
}
