using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactSupportData // 이런식으로 스킬에 필요한 데이터 묶어서 보내기
{
    public List<Vector3> contactPos;
    public List<Entity> contactEntity;

    public ContactSupportData(List<Entity> contactEntity, List<Vector3> contactPos)
    {
        this.contactEntity = contactEntity;
        this.contactPos = contactPos;
    }
}

[System.Serializable]
public struct ContactData
{
    public GameObject go;
    public Vector3 pos;

    public ContactData(GameObject go, Vector3 pos)
    {
        this.go = go;
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
        ContactData contactData = new ContactData(col.gameObject, col.contacts[0].point);
        m_contactDatas.Add(contactData);
    }

    public void CallWhenCollisionExit(Collision2D col)
    {
        for (int i = 0; i < m_contactDatas.Count; i++)
        {
            if (m_contactDatas[i].go == col.gameObject)
            {
                m_contactDatas.RemoveAt(i);
                return;
            }
        }
    }

    bool CheckCorrectEntity(EntityTag tag)
    {
        for (int i = 0; i < m_entityTags.Count; i++)
        {
            if (m_entityTags[i] == tag) return true;
        }

        return false;
    }

    ContactSupportData ReturnContactSupportData()
    {
        List<Vector3> pos = new List<Vector3>();
        List<Entity> entities = new List<Entity>();

        for (int i = 0; i < m_contactDatas.Count; i++)
        {
            m_contactDatas[i].go.TryGetComponent(out Entity entity);
            if (entity == null || !CheckCorrectEntity(entity.InheritedTag)) continue;

            pos.Add(m_contactDatas[i].pos);
            entities.Add(entity);
        }

        ContactSupportData supportData = new ContactSupportData(entities, pos);
        return supportData;
    }
}
