using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class DamageMethod<T> : BaseMethod<T>
{
    protected DamageSupportData damageSkillData;

    protected bool DamageToEntity(GameObject me, Transform enemy, SkillData data)
    {
        enemy.TryGetComponent(out IHealth health);

        if (health == null || data.CanHitSkill(health.ReturnEntityTag()) == false) return false;

        health.UnderAttack(data.Damage, -(me.transform.position - enemy.position).normalized, data.KnockBackThrust);
        return true;
    }

    public override void Init(SkillData data) // 여기서 불러옴
    {
        //damageStat = DatabaseManager.Instance.db
    }
}

public class DamageToContactors : DamageMethod<List<ContactData>>
{
    public override void Execute(SkillSupportData supportData, List<ContactData> contactDatas)
    {
        for (int i = 0; i < contactDatas.Count; i++)
        {
            DamageToEntity(supportData.Caster, contactDatas[i].transform, supportData.Data); // 이렇게 진행
            PlayEffect(contactDatas[i].transform, damageSkillData.effectDatas[EffectName.PunchEffect]);
        }
    }
}

public class DamageToRaycastHit : DamageMethod<RaycastHit2D[]>
{
    public override void Execute(SkillSupportData supportData, RaycastHit2D[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            DamageToEntity(supportData.Caster, targets[i].transform, supportData.Data); // 이렇게 진행
            PlayEffect(supportData.Caster.transform.position, damageSkillData.effectDatas[EffectName.PunchEffect]);
        }

        PlayEffect(supportData.Caster.transform, damageSkillData.effectDatas[EffectName.AttackSkillEffect]);
    }
}

public class DamageToLaserHit : DamageMethod<RaycastHit2D[]>
{
    [SerializeField]
    float hitEffectDisableTime = 1.5f;

    [SerializeField]
    float maxDistance = 20;

    [SerializeField]
    List<string> blockedTag = new List<string>();

    public override void Execute(SkillSupportData supportData, RaycastHit2D[] hits)
    {
        blockedTag.Add("Wall");

        List<Vector3> hitPos = new List<Vector3>();
        List<Vector3> hitEffectPos = new List<Vector3>();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == null || hits[i].transform == supportData.Caster.transform) continue;

            bool isBlocked = false;

            for (int j = 0; j < blockedTag.Count; j++)
            {
                if (hits[i].transform.tag == blockedTag[j])
                {
                    hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
                    hitEffectPos.Add(hits[i].point);
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked) break;

            if (DamageToEntity(supportData.Caster, hits[i].transform, supportData.Data) == false) continue;

            hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
            hitEffectPos.Add(hits[i].point);

            break;
        }

        if (hitPos.Count == 0)
        {
            hitPos.Add(supportData.Data.Directions[supportData.TickCount - 1] * maxDistance / 2);
        }

        for (int i = 0; i < hitEffectPos.Count; i++)
        {
            PlayEffect(hitEffectPos[i], damageSkillData.effectDatas[EffectName.HitSurfaceEffect]);
        }

        PlayEffect(supportData.Caster.transform, damageSkillData.effectDatas[EffectName.AttackSkillEffect]);
        SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, supportData.Data.SfxName, supportData.Data.Volume);
    }
}