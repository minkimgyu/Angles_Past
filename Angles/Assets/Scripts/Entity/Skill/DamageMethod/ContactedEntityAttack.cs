using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "ContactedEntityAttack", menuName = "Scriptable Object/DamageMethod/ContactedEntityAttack", order = int.MaxValue)]
public class ContactedEntityAttack : DamageMethod
{
    [SerializeField]
    bool onlyFirstOneAttack;

    public override void Execute(DamageSupportData damageSupportData)
    {
        //damageSupportData.Caster.TryGetComponent(out ContactComponent contact);
        //if (contact == null) return;

        //ContactSupportData contactSupportData = contact.ReturnContactSupportData();

        //for (int i = 0; i < contactSupportData.ContactEntity.Count; i++)
        //{
        //    if(DamageToEntity(damageSupportData.Caster, contactSupportData.ContactEntity[i].transform, damageSupportData.Me.Data) == true)
        //    {
        //        // ����Ʈ ���� --> supportData.ContactPos�� ������Ű��
        //        BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
        //        if (effectPlayer == null) continue;

        //        damageSupportData.Me.EffectPlayer = effectPlayer; // Effect ���� �ʱ�ȭ

        //        effectPlayer.Init(contactSupportData.ContactPos[i], damageSupportData.Me.Data.DisableTime);
        //        effectPlayer.PlayEffect();

        //        if (onlyFirstOneAttack) break;
        //    }
        //}
    }
}