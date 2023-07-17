using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "FixToContactors", menuName = "Scriptable Object/PositionMethod/FixToContactors", order = int.MaxValue)]
public class FixToContactors : PositionMethod
{
    public override void DoUpdate(BasicSkill me)
    {
        me.transform.position = me.PosTr.position; // 계속 고정시킴
    }

    public override void Init(Transform caster, BasicSkill me)
    {
        caster.TryGetComponent(out ContactComponent contact);
        if (contact == null) return;

        ContactSupportData supportData = contact.ReturnContactSupportData();

        me.PosTr = supportData.ContactEntity[0].transform;
    }
}
