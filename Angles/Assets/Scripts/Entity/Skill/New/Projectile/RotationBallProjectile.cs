using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBallProjectile : ContactableObject
{
    Transform m_caster;

    // update�� �ֺ� ���� ��� ���� --> �̰͵� GameObject - Caster �ʿ���
    protected override void DoUpdate()
    {
        // m_caster
    }

    public override void ResetObject(Transform caster, Vector3 pos)
    {
        base.ResetObject(caster, pos);
        m_caster = caster;
        transform.position = pos;
    }

    public void Inintialize(float disableTime, string[] skillNames, string[] hitTargetTag)
    {
        Inintialize(disableTime, skillNames);
        this.hitTargetTag = hitTargetTag;
    }
}