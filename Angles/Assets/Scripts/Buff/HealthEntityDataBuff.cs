using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntityDataBuff : PassiveBuff<HealthEntityData.BuffVariation>
{
    public HealthEntityDataBuff(BuffData data) : base(data)
    {
    }

    HealthEntityData storedHealthEntityData;

    public override IBuff CreateCopy(BuffData data)
    {
        return new HealthEntityDataBuff(data);
    }

    public override void OnEnd()
    {
        storedHealthEntityData.RemoveBuff(variationData);

        //effectComponent.ReturnBaseColor();
    }

    public override void OnStart(GameObject caster)
    {
        caster.TryGetComponent(out IHealth health);

        if (health == null) isFinished = true;
        else storedHealthEntityData = health.ReturnHealthEntityData();


        variationData = DatabaseManager.instance.UtilizationDB.HealthEntityDataVariation[Data.VariationName];
        storedHealthEntityData.ApplyBuff(variationData);

        //effectComponent.ChangeColor(buffEffectColor); // --> 수정점 생각해보기
    }
}

public class HealthEntityDataTimerBuff : TimeBuff<HealthEntityData.BuffVariation>
{
    public HealthEntityDataTimerBuff(BuffData data) : base(data)
    {
    }

    HealthEntityData storedHealthEntityData;

    public override IBuff CreateCopy(BuffData data)
    {
        return new HealthEntityDataTimerBuff(data);
    }

    public override void OnEnd()
    {
        storedHealthEntityData.RemoveBuff(variationData);
    }

    public override void OnStart(GameObject caster)
    {
        caster.TryGetComponent(out IHealth health);

        if (health == null) isFinished = true;
        else storedHealthEntityData = health.ReturnHealthEntityData();


        variationData = DatabaseManager.instance.UtilizationDB.HealthEntityDataVariation[Data.VariationName];
        storedHealthEntityData.ApplyBuff(variationData);
    }

    public override void ApplyTickEffect()
    {
    }
}