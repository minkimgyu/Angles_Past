using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SpecifyLocation
{
    protected Transform m_posTr;
    protected Vector3 m_pos;
    protected bool m_isFix;

    protected Transform m_caster;
    public GameObject Caster { get { return m_caster.gameObject; } }

    public virtual void Init(Transform caster, bool isFix)
    {
        m_caster = caster;
    }

    public virtual Vector3 ReturnPos()
    {
        if (m_isFix) return m_posTr.position;
        else return m_pos;
    }
}

public class LocationToCaster : SpecifyLocation
{
    public override void Init(Transform caster, bool isFix)
    {
        base.Init(caster, isFix);

        m_isFix = isFix;

        if (m_isFix) m_posTr = caster;
        else m_pos = caster.position;
    }
}

public class LocationToContactor : SpecifyLocation
{
    public override void Init(Transform caster, bool isFix)
    {
        base.Init(caster, isFix);

        caster.TryGetComponent(out ContactComponent contact);
        if (contact == null) return;

        List<ContactData> supportData = contact.ReturnContactSupportData();

        m_isFix = isFix;

        if (m_isFix) m_posTr = supportData[0].transform;
        else m_pos = supportData[0].transform.position;
    }
}
