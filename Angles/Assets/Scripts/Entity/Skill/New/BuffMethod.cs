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


    protected void ApplyBuff(Transform target)
    {
        if (m_nowApply) AddBuffToEntity(target);
        else RemoveBuffToEntity(target);
    }

    protected bool AddBuffToEntity(Transform target)
    {
        if (IsTarget(target, out IAvatar avatar) == false) return false;

        //target.TryGetComponent(out IAvatar avatar);
        //if (avatar == null || CanHitSkill(avatar.ReturnEntityTag()) == false) return false;
        // --> 타겟이 맞는지 확인

        for (int i = 0; i < m_buffNames.Length; i++)
        {
            avatar.AddBuffToController(m_buffNames[i]);

            //BuffData data = DatabaseManager.Instance.UtilizationDB.ReturnBuffData(m_buffNames[i]); // 추후 버프 수정
            //if (data == null) continue;

            //health.AddBuffToController(data);
        }

        return true;
    }

    protected bool RemoveBuffToEntity(Transform target)
    {
        if (IsTarget(target, out IAvatar avatar) == false) return false;

        //target.TryGetComponent(out IAvatar avatar);
        //if (avatar == null || CanHitSkill(avatar.ReturnEntityTag()) == false) return false;
        // --> 타겟이 맞는지 확인

        for (int i = 0; i < m_buffNames.Length; i++)
        {
            avatar.RemoveBuffToController(m_buffNames[i]);

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
            ApplyBuff(contactDatas[i].transform);
            PlayEffect(contactDatas[i].transform, EffectCondition.BuffEffect);
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
            ApplyBuff(targets[i].transform);
            PlayEffect(targets[i].transform, EffectCondition.BuffEffect);
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
        ApplyBuff(supportData.Caster.transform);
        PlayEffect(supportData.Caster.transform, EffectCondition.BuffEffect);
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
        ApplyBuff(target);

        if (m_effectDatas.Count == 0) return;
        PlayEffect(supportData.Caster.transform, EffectCondition.BuffEffect);
    }
}