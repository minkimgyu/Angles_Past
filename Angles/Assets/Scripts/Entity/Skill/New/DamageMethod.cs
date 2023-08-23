using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TargetMethod<T> : BaseMethod<T>
{
    EntityTag[] m_hitTarget;

    public TargetMethod(EntityTag[] hitTarget, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(effectDatas, soundDatas)
    {
        m_hitTarget = hitTarget;
    }

    protected bool CanHitSkill(EntityTag tag)
    {
        for (int i = 0; i < m_hitTarget.Length; i++)
        {
            if (m_hitTarget[i] == tag)
            {
                return true;
            }
        }

        return false;
    }
}

abstract public class DamageMethod<T> : TargetMethod<T>
{
    float m_knockBackThrust;
    float m_damage;
    

    public DamageMethod(EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(hitTarget, effectDatas, soundDatas)
    {
        m_knockBackThrust = knockBackThrust;
        m_damage = damage;
    }

    protected bool DamageToEntity(GameObject me, Transform enemy)
    {
        enemy.TryGetComponent(out IHealth health);

        if (health == null || CanHitSkill(health.ReturnEntityTag()) == false) return false;

        health.UnderAttack(m_damage, -(me.transform.position - enemy.position).normalized, m_knockBackThrust);
        return true;
    }
}

public class DamageToContactors : DamageMethod<List<ContactData>>
{
    public DamageToContactors(EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas) 
        : base(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) // 이런 식으로 생성자에서 값을 받아서 Execute 진행
    {
    }

    public override void Execute(SkillSupportData supportData, List<ContactData> contactDatas)
    {
        for (int i = 0; i < contactDatas.Count; i++)
        {
            DamageToEntity(supportData.Caster, contactDatas[i].transform); // 이렇게 진행
            PlayEffect(contactDatas[i].transform.position, m_effectDatas[EffectCondition.HitSurfaceEffect]);
        }
    }
}

public class DamageToRaycastHit : DamageMethod<RaycastHit2D[]>
{
    public DamageToRaycastHit(EntityTag[] hitTarget, float knockBackThrust, float damage, float[] skillScalePerTicks,
        EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) // 이런 식으로 생성자에서 값을 받아서 Execute 진행
    {
    }

    public override void Execute(SkillSupportData supportData, RaycastHit2D[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            DamageToEntity(supportData.Caster, targets[i].transform); // 이렇게 진행
            PlayEffect(supportData.Caster.transform.position, m_effectDatas[EffectCondition.HitSurfaceEffect]);
        }

        PlayEffect(supportData.Caster.transform, m_effectDatas[EffectCondition.AttackEffect]);
    }
}

public class DamageToLaserHit : DamageMethod<RaycastHit2D[]>
{
    float m_laserMaxDistance = 20;
    List<string> m_blockedTag = new List<string>(); //m_blockedTag.Add("Wall");
    List<Vector2> m_directionPerTick;

    public DamageToLaserHit(EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas, 
        float laserMaxDistance, List<string> blockedTag, List<Vector2> directionPerTick) : base(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) // 이런 식으로 생성자에서 값을 받아서 Execute 진행
    {
        m_laserMaxDistance = laserMaxDistance;
        m_blockedTag = blockedTag;
        m_directionPerTick = directionPerTick;
    }


    public override void Execute(SkillSupportData supportData, RaycastHit2D[] hits)
    {
        List<Vector3> hitPos = new List<Vector3>();
        List<Vector3> hitEffectPos = new List<Vector3>();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == null || hits[i].transform == supportData.Caster.transform) continue;

            bool isBlocked = false;

            for (int j = 0; j < m_blockedTag.Count; j++)
            {
                if (hits[i].transform.tag == m_blockedTag[j])
                {
                    hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
                    hitEffectPos.Add(hits[i].point);
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked) break;

            if (DamageToEntity(supportData.Caster, hits[i].transform) == false) continue;

            hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
            hitEffectPos.Add(hits[i].point);

            break;
        }

        if (hitPos.Count == 0)
        {
            hitPos.Add(m_directionPerTick[supportData.TickCount - 1] * m_laserMaxDistance / 2);
        }

        for (int i = 0; i < hitEffectPos.Count; i++)
        {
            PlayEffect(hitEffectPos[i], m_effectDatas[EffectCondition.HitSurfaceEffect]);
        }

        PlayEffect(supportData.Caster.transform, m_effectDatas[EffectCondition.AttackEffect]);
        //SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, supportData.Data.SfxName, supportData.Data.Volume);
    }
}