using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ContactData
{
    public Transform transform;
    public Vector3 position;

    public ContactData(Transform tr, Vector3 pos)
    {
        this.transform = tr;
        this.position = pos;
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
        ContactData contactData = m_contactDatas.Find(x => x.transform == col.transform);
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
            m_contactDatas[i].transform.TryGetComponent(out Entity entity);
            if (entity == null || !CheckCorrectEntity(entity.InheritedTag)) continue;

            tmpContactDatas.Add(m_contactDatas[i]);
        }

        return tmpContactDatas;
    }
}
