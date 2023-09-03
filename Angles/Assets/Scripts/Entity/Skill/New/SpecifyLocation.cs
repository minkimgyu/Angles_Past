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

    public abstract void Init(GameObject caster);

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

    public override void Init(GameObject caster)
    {
        if (m_isFix) m_posTr = caster.transform;
        else m_pos = caster.transform.position;
    }
}

public class LocationToContactor : SpecifyLocation
{
    public LocationToContactor(bool isFix) : base(isFix)
    {
    }

    public override void Init(GameObject caster)
    {
        caster.TryGetComponent(out ContactComponent contact);
        if (contact == null) return;

        List<ContactData> supportData = contact.ReturnContactSupportData();
        if (supportData.Count == 0) return;

        if (m_isFix) m_posTr = supportData[0].transform;
        else m_pos = supportData[0].transform.position;
    }
}
