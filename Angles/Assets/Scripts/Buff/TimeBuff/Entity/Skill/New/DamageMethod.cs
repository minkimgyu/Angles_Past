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

    bool CanHitSkill(EntityTag tag)
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

    protected bool IsTarget(Transform enemy, out IAvatar avatar)
    {
        enemy.TryGetComponent(out IAvatar tmpAvatar);

        avatar = tmpAvatar;

        if (avatar == null || CanHitSkill(avatar.ReturnEntityTag()) == false) return false;

        return true;
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

    protected bool DamageToEntity(IHealth health, GameObject me, Transform enemy)
    {
        health.UnderAttack(m_damage, -(me.transform.position - enemy.position).normalized, m_knockBackThrust);
        return true;
    }
}

public class DamageToContactors : DamageMethod<List<ContactData>>
{
    public DamageToContactors(EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas) 
        : base(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) // �̷� ������ �����ڿ��� ���� �޾Ƽ� Execute ����
    {
    }

    public override void Execute(SkillSupportData supportData, List<ContactData> contactDatas)
    {
        PlaySound(supportData.Pos, EffectCondition.AttackEffect);

        for (int i = 0; i < contactDatas.Count; i++)
        {
            if (IsTarget(contactDatas[i].transform, out IAvatar health) == false) continue;

            PlayEffect(contactDatas[i].position, EffectCondition.HitSurfaceEffect);
            DamageToEntity(health, supportData.Caster, contactDatas[i].transform); // �̷��� ����
        }
    }
}

public class DamageUsingRaycast : DamageMethod<RaycastHit2D[]> // ��ų ��ġ�� ����Ʈ ����
{
    float[] skillScalePerTicks;
    bool IsAttackEffectOnCasterPosition;

    public DamageUsingRaycast(EntityTag[] hitTarget, float knockBackThrust, float damage, float[] skillScalePerTicks, bool IsAttackEffectOnCasterPosition,
        EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) // �̷� ������ �����ڿ��� ���� �޾Ƽ� Execute ����
    {
        this.skillScalePerTicks = skillScalePerTicks;
        this.IsAttackEffectOnCasterPosition = IsAttackEffectOnCasterPosition;
    }

    public override void Execute(SkillSupportData supportData, RaycastHit2D[] targets)
    {
        if(IsAttackEffectOnCasterPosition) PlayEffect(supportData.Caster.transform, EffectCondition.AttackEffect, skillScalePerTicks[supportData.TickCount]);
        else PlayEffect(supportData.Pos, EffectCondition.AttackEffect, skillScalePerTicks[supportData.TickCount]);

        PlaySound(supportData.Pos, EffectCondition.AttackEffect);

        for (int i = 0; i < targets.Length; i++)
        {
            if (IsTarget(targets[i].transform, out IAvatar health) == false) continue;

            PlayEffect(targets[i].transform, EffectCondition.HitSurfaceEffect);
            DamageToEntity(health, supportData.Caster, targets[i].transform); // �̷��� ����
        }
    }
}

public class DamageUsingLaserHit : DamageMethod<RaycastHit2D[]>
{
    float m_laserMaxDistance = 20;
    EntityTag[] m_blockedTag; //m_blockedTag.Add("Wall");
    Vector3[] m_directionPerTick;

    public DamageUsingLaserHit(EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas, 
        float laserMaxDistance, EntityTag[] blockedTag) : base(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) // �̷� ������ �����ڿ��� ���� �޾Ƽ� Execute ����
    {
        m_laserMaxDistance = laserMaxDistance;
        m_blockedTag = blockedTag;
    }


    public override void Execute(SkillSupportData supportData, RaycastHit2D[] hits)
    {
        List<Vector3> hitPos = new List<Vector3>();
        List<Vector3> hitEffectPos = new List<Vector3>();

        m_directionPerTick = supportData.Caster.GetComponent<ISpecifyDirection>().Directions; // ������ ���� GetComponent�� �̿��ؼ� �޾ƿ´�.

        PlaySound(supportData.Pos, EffectCondition.AttackEffect);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == null || hits[i].transform == supportData.Caster.transform) continue;

            bool isBlocked = false;

            for (int j = 0; j < m_blockedTag.Length; j++)
            {
                if (hits[i].transform.tag == m_blockedTag[j].ToString())
                {
                    hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
                    hitEffectPos.Add(hits[i].point);
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked) break;

            if (IsTarget(hits[i].transform, out IAvatar health) == false) continue;

            DamageToEntity(health, supportData.Caster, hits[i].transform); // �̷��� ����
            // ��� �޼ҵ� �߰�

            //if (DamageToEntity(supportData.Caster, hits[i].transform) == false) continue;

            hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
            hitEffectPos.Add(hits[i].point);

            break;
        }

        if (hitPos.Count == 0)
        {
            hitPos.Add(m_directionPerTick[supportData.TickCount] * m_laserMaxDistance / 2); // supportData.TickCount - 1 �̰� -1 ������
        }

        for (int i = 0; i < hitEffectPos.Count; i++)
        {
            if (m_effectDatas.Count == 0) continue;
            PlayEffect(hitEffectPos[i], EffectCondition.HitSurfaceEffect);
        }

        //Vector3 posVec, Vector3[] pos


        if (m_effectDatas.Count == 0) return;
        PlayEffect(supportData.Pos, hitPos, EffectCondition.AttackEffect);
        //SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, supportData.Data.SfxName, supportData.Data.Volume);
    }
}