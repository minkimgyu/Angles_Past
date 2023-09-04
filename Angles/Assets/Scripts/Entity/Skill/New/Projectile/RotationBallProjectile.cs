using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBallProjectile : ContactableObject
{
    Transform m_caster;

    float speed;
    float tmpAngle;

    float angle = 0;
    float distanceFromCaster;

    // update�� �ֺ� ���� ��� ���� --> �̰͵� GameObject - Caster �ʿ���
    protected override void DoUpdate()
    {
        if (m_caster == null) return;

        angle = speed * Time.time + tmpAngle;
        Vector3 offset = new Vector2(Mathf.Cos(angle * 0.0174533f), Mathf.Sin(angle * 0.0174533f)) * distanceFromCaster;
        transform.position = m_caster.position + offset;
    }

    public override void ResetObject(Transform caster, float tmpAngle, float distanceFromCaster)
    {
        m_caster = caster;
        this.tmpAngle = tmpAngle;
        this.distanceFromCaster = distanceFromCaster;
    }

    public void Inintialize(float disableTime, string[] skillNames, EntityTag[] hitTargetTag, float speed)
    {
        Inintialize(disableTime, skillNames);
        this.hitTargetTag = hitTargetTag;
        this.speed = speed;
    }
}