using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class ContactSupportData // 이런식으로 스킬에 필요한 데이터 묶어서 보내기
//{
//    List<Vector3> contactPos;
//    public List<Vector3> ContactPos { get { return contactPos; } }

//    List<Entity> contactEntity;
//    public List<Entity> ContactEntity { get { return contactEntity; } }

//    public ContactSupportData(List<Entity> contactEntity, List<Vector3> contactPos)
//    {
//        this.contactEntity = contactEntity;
//        this.contactPos = contactPos;
//    }
//}

[System.Serializable]
public struct ContactData
{
    public Transform tr;
    public Vector3 pos;

    public ContactData(Transform tr, Vector3 pos)
    {
        this.tr = tr;
        this.pos = pos;
    }
}

public class ContactComponent : MonoBehaviour
{
    [SerializeField]
    List<ContactData> m_contactDatas = new List<ContactData>();

    [SerializeField]
    List<EntityTag> m_entityTags;

    public void CallWhenCollisionEnter(Collision2D col)
    {
        ContactData contactData = new ContactData(col.transform, col.contacts[0].point);
        m_contactDatas.Add(contactData);
    }

    public void CallWhenCollisionExit(Collision2D col)
    {
        ContactData contactData = m_contactDatas.Find(x => x.tr == col.transform);
        m_contactDatas.Remove(contactData);
    }

    bool CheckCorrectEntity(EntityTag tag)
    {
        for (int i = 0; i < m_entityTags.Count; i++)
        {
            if (m_entityTags[i] == tag) return true;
        }

        return false;
    }

    public List<ContactData> ReturnContactSupportData()
    {
        List<ContactData> tmpContactDatas = new List<ContactData>();

        for (int i = 0; i < m_contactDatas.Count; i++)
        {
            m_contactDatas[i].tr.TryGetComponent(out Entity entity);
            if (entity == null || !CheckCorrectEntity(entity.InheritedTag)) continue;

            tmpContactDatas.Add(m_contactDatas[i]);
        }

        return tmpContactDatas;
    }
}
