using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public List<ContactData> ReturnContactSupportData()
    {
        return m_contactDatas.ToList();
    }
}
