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
    SpawnRotationBall spawnRotationBall;

    // update로 주변 도는 기능 구현 --> 이것도 GameObject - Caster 필요함
    protected override void DoUpdate()
    {
        if (m_caster == null) return;

        angle = speed * Time.time + tmpAngle;
        Vector3 offset = new Vector2(Mathf.Cos(angle * 0.0174533f), Mathf.Sin(angle * 0.0174533f)) * distanceFromCaster;
        transform.position = m_caster.position + offset;
    }

    public override void ResetObject(Transform caster, float tmpAngle, float distanceFromCaster, SpawnRotationBall spawnRotationBall)
    {
        m_caster = caster;
        this.tmpAngle = tmpAngle;
        this.distanceFromCaster = distanceFromCaster;
        this.spawnRotationBall = spawnRotationBall;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (spawnRotationBall == null) return;
        spawnRotationBall.RemoveSpawnObject(this);
    }

    public void Inintialize(float disableTime, string[] skillNames, EntityTag[] hitTargetTag, float speed)
    {
        Inintialize(disableTime, skillNames);
        this.hitTargetTag = hitTargetTag;
        this.speed = speed;
    }
}