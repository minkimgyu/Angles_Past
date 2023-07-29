using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntitySpeedBuff : PassiveBuff
{
    PlayerData entityData;

    [SerializeField]
    float speed;

    [SerializeField]
    float readySpeed;

    [SerializeField]
    Color buffEffectColor;

    public override void OnEnd(BuffEffectComponent effectComponent)
    {
        //if(m_effectPlayer != null)
        //{
        //    m_effectPlayer.StopEffect();
        //}

        effectComponent.ReturnBaseColor();

        entityData.Speed.OriginValue -= speed;
        entityData.ReadySpeed.OriginValue -= readySpeed;


        gameObject.SetActive(false);
    }

    public override void OnStart(GameObject caster, BuffEffectComponent effectComponent)
    {
        caster.TryGetComponent(out IBuff<PlayerData> buffData);

        if (buffData == null) isFinished = true;
        else entityData = buffData.GetData();

        entityData.Speed.OriginValue += speed;
        entityData.ReadySpeed.OriginValue += readySpeed;

        // 수치에 영향이 없으면 이펙트 생성 X
        //if (entityData.Speed.IsOutInterval() || entityData.ReadySpeed.IsOutInterval()) return;

        effectComponent.ChangeColor(buffEffectColor);

        //BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
        //if (effectPlayer != null)
        //{
        //    m_effectPlayer = effectPlayer;
        //    effectPlayer.Init(caster.transform, 1000f);
        //    effectPlayer.PlayEffect();
        //}
    }
}
