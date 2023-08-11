using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntityDataBuff : PassiveBuff<HealthEntityData>
{
    public HealthEntityDataBuff(BuffData data) : base(data)
    {
    }

    HealthEntityData healthEntityData;

    HealthEntityData.HealthEntityDataVariation variationData;

    [SerializeField]
    float speed;

    [SerializeField]
    Color buffEffectColor;

    public override IBuff CreateCopy(BuffData data)
    {
        return new HealthEntityDataBuff(data);
    }

    public override void OnEnd(BuffEffectComponent effectComponent)
    {
        healthEntityData.RemoveBuff(variationData);

        effectComponent.ReturnBaseColor();
    }

    public override void OnStart(GameObject caster, BuffEffectComponent effectComponent)
    {
        caster.TryGetComponent(out IHealth health);

        if (health == null) isFinished = true;
        else healthEntityData = health.ReturnHealthEntityData();

        variationData = new();
        healthEntityData.ApplyBuff(variationData);

        effectComponent.ChangeColor(buffEffectColor); // --> 수정점 생각해보기
    }
}