using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntitySpeedBuff : PassiveBuff
{
    HealthEntityData healthEntityData;

    [SerializeField]
    float speed;

    [SerializeField]
    Color buffEffectColor;

    public override void OnEnd(BuffEffectComponent effectComponent)
    {
        healthEntityData.Speed.OriginValue -= speed;
        effectComponent.ReturnBaseColor();
        gameObject.SetActive(false);
    }

    public override void OnStart(GameObject caster, BuffEffectComponent effectComponent)
    {
        caster.TryGetComponent(out IHealth health);

        if (health == null) isFinished = true;
        else healthEntityData = health.ReturnHealthEntityData();

        healthEntityData.Speed.OriginValue += speed;
        effectComponent.ChangeColor(buffEffectColor); // --> 수정점 생각해보기
    }
}
