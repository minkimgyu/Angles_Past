using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "ContactedEntityAttack", menuName = "Scriptable Object/DamageMethod/ContactedEntityAttack", order = int.MaxValue)]
public class ContactedEntityAttack : DamageMethod
{
    public override void Attack(DamageSupportData damageSupportData)
    {
        damageSupportData.Caster.TryGetComponent(out ContactComponent contact);
        if (contact == null) return;

        ContactSupportData contactSupportData = contact.ReturnContactSupportData();

        for (int i = 0; i < contactSupportData.ContactEntity.Count; i++)
        {
            if(DamageToEntity(damageSupportData.Caster, contactSupportData.ContactEntity[i].transform, damageSupportData.Data) == true)
            {
                // ����Ʈ ���� --> supportData.ContactPos�� ������Ű��

                BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
                effectPlayer.Init(contactSupportData.ContactEntity[i].transform);
            }
        }
    }
}
