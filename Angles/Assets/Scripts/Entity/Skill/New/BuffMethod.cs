using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMethod<T> : BaseMethod<T>
{
    protected BuffSupportData buffSupportData;

    protected bool BuffToEntity(SkillSupportData supportData, Transform target)
    {
        target.TryGetComponent(out IHealth health);
        if (health == null || supportData.Data.CanHitSkill(health.ReturnEntityTag()) == false) return false;
        // --> 타겟이 맞는지 확인


        BuffComponent buffComponent = health.ReturnBuffComponent(); // 버프 컴포넌트 반환

        for (int i = 0; i < buffSupportData.buffNames.Count; i++)
        {
            BuffData data = DatabaseManager.Instance.UtilizationDB.ReturnBuffData(buffSupportData.buffNames[i]);
            if (data == null) continue;

            buffComponent.AddBuff(data);
        }
        
        return true;
    }

    public override void Init(SkillData data)
    {
        
    }
}

public class BuffToContactors : BuffMethod<List<ContactData>>
{
    public override void Execute(SkillSupportData supportData, List<ContactData> contactDatas)
    {
        for (int i = 0; i < contactDatas.Count; i++)
        {
            BuffToEntity(supportData, contactDatas[i].transform);
            PlayEffect(contactDatas[i].transform, buffSupportData.effectDatas[EffectName.BuffEffect]);
        }
    }
}

public class BuffToRaycastHit : BuffMethod<RaycastHit2D[]>
{
    public override void Execute(SkillSupportData supportData, RaycastHit2D[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            BuffToEntity(supportData, targets[i].transform);
            PlayEffect(targets[i].transform, buffSupportData.effectDatas[EffectName.BuffEffect]);
        }
    }
}

public class BuffToCaster : BuffMethod<bool>
{
    public override void Execute(SkillSupportData supportData)
    {
        BuffToEntity(supportData, supportData.Caster.transform);
        PlayEffect(supportData.Caster.transform, buffSupportData.effectDatas[EffectName.BuffEffect]);
    }
}