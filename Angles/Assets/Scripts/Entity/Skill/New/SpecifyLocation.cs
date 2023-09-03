using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
abstract public class SpecifyLocation
{
    protected Transform m_posTr;

    protected Vector3 m_pos;

    protected bool m_isFix;

    public SpecifyLocation(bool isFix)
    {
        m_isFix = isFix;
    }

    public virtual void Init(GameObject caster, out Transform target, out float scale) { target = null; scale = 1; }

    public virtual Vector3 ReturnPos()
    {
        if (m_isFix) return m_posTr.position;
        else return m_pos;
    }
}

public class LocationToCaster : SpecifyLocation
{
    public LocationToCaster(bool isFix) : base(isFix)
    {
    }

    public override void Init(GameObject caster, out Transform target, out float scale)
    {
        target = caster.transform;
        scale = caster.transform.localScale.x;

        if (m_isFix) m_posTr = caster.transform;
        else m_pos = caster.transform.position;
    }
}

public class LocationToContactor : SpecifyLocation
{
    EntityTag[] entityTags;

    public LocationToContactor(bool isFix, EntityTag[] entityTags) : base(isFix)
    {
        this.entityTags = entityTags;
    }

    public override void Init(GameObject caster, out Transform target, out float scale)
    {
        caster.TryGetComponent(out ContactComponent contact);
        if (contact == null)
        {
            target = null;
            scale = 1;
            return;
        }

        List<ContactData> supportData = contact.ReturnContactSupportData();
        if (supportData.Count == 0)
        {
            target = null;
            scale = 1;
            return;
        }


        for (int i = 0; i < supportData.Count; i++)
        {
            supportData[i].transform.TryGetComponent(out IAvatar avatar);
            if (avatar == null) continue;

            EntityTag targetTag = avatar.ReturnEntityTag();

            for (int j = 0; j < entityTags.Length; j++)
            {
                if (targetTag == entityTags[i])
                {
                    if (m_isFix) m_posTr = supportData[i].transform;
                    else m_pos = supportData[i].transform.position;
                    // 이렇게 해당 태그를 찾아서 변수에 넣어주기

                    target = m_posTr;
                    scale = m_posTr.localScale.x;
                    return;
                }
            }
        }

        target = null;
        scale = 1;
    }
}
