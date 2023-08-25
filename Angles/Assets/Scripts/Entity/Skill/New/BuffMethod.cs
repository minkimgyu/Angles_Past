using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMethod<T> : TargetMethod<T>
{
    bool m_nowApply;
    public string[] m_buffNames;

    public BuffMethod(EntityTag[] hitTarget, bool nowApply, string[] buffNames, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
        : base(hitTarget, effectDatas, soundDatas)
    {
        m_nowApply = nowApply;
        m_buffNames = buffNames;
    }


    protected void ApplyBuff(SkillSupportData supportData, Transform target)
    {
        if (m_nowApply) AddBuffToEntity(supportData, target);
        else RemoveBuffToEntity(supportData, target);
    }

    protected bool AddBuffToEntity(SkillSupportData supportData, Transform target)
    {
        target.TryGetComponent(out IHealth health);
        if (health == null || CanHitSkill(health.ReturnEntityTag()) == false) return false;
        // --> 타겟이 맞는지 확인

        for (int i = 0; i < m_buffNames.Length; i++)
        {
            //BuffData data = DatabaseManager.Instance.UtilizationDB.ReturnBuffData(m_buffNames[i]); // 추후 버프 수정
            //if (data == null) continue;

            //health.AddBuffToController(data);
        }

        return true;
    }

    protected bool RemoveBuffToEntity(SkillSupportData supportData, Transform target)
    {
        target.TryGetComponent(out IHealth health);
        if (health == null || CanHitSkill(health.ReturnEntityTag()) == false) return false;
        // --> 타겟이 맞는지 확인

        for (int i = 0; i < m_buffNames.Length; i++)
        {
            //BuffData data = DatabaseManager.Instance.UtilizationDB.ReturnBuffData(buffSupportData.buffNames[i]); // 추후 버프 수정
            //if (data == null) continue;

            //health.RemoveBuffToController(data);
        }

        return true;
    }
}

public class BuffToContactors : BuffMethod<List<ContactData>>
{
    public BuffToContactors(EntityTag[] hitTarget, bool nowApply, string[] buffNames, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
        : base(hitTarget, nowApply, buffNames, effectDatas, soundDatas)
    {
    }

    public override void Execute(SkillSupportData supportData, List<ContactData> contactDatas)
    {
        for (int i = 0; i < contactDatas.Count; i++)
        {
            ApplyBuff(supportData, contactDatas[i].transform);
            PlayEffect(contactDatas[i].transform, m_effectDatas[EffectCondition.BuffEffect]);
        }
    }
}

public class BuffToRaycastHit : BuffMethod<RaycastHit2D[]>
{
    public BuffToRaycastHit(EntityTag[] hitTarget, bool nowApply, string[] buffNames, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
        : base(hitTarget, nowApply, buffNames, effectDatas, soundDatas)
    {
    }

    public override void Execute(SkillSupportData supportData, RaycastHit2D[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            ApplyBuff(supportData, targets[i].transform);
            PlayEffect(targets[i].transform, m_effectDatas[EffectCondition.BuffEffect]);
        }
    }
}

public class BuffToCaster : BuffMethod<GameObject>
{
    public BuffToCaster(EntityTag[] hitTarget, bool nowApply, string[] buffNames, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
        : base(hitTarget, nowApply, buffNames, effectDatas, soundDatas)
    {
    }

    public override void Execute(SkillSupportData supportData)
    {
        ApplyBuff(supportData, supportData.Caster.transform);
        PlayEffect(supportData.Caster.transform, m_effectDatas[EffectCondition.BuffEffect]);
    }
}

public class BuffToTarget : BuffMethod<Transform>
{
    public BuffToTarget(EntityTag[] hitTarget, bool nowApply, string[] buffNames, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
        : base(hitTarget, nowApply, buffNames, effectDatas, soundDatas)
    {
    }

    public override void Execute(SkillSupportData supportData, Transform target)
    {
        ApplyBuff(supportData, target);
        PlayEffect(supportData.Caster.transform, m_effectDatas[EffectCondition.BuffEffect]);
    }
}