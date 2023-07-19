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

    public override void OnEnd()
    {
        if(m_effectPlayer != null)
        {
            m_effectPlayer.StopEffect();
        }

        entityData.Speed.OriginValue -= speed;
        entityData.ReadySpeed.OriginValue -= readySpeed;

        gameObject.SetActive(false);
    }

    public override void OnStart(GameObject caster)
    {
        caster.TryGetComponent(out IBuff<PlayerData> buffData);

        if (buffData == null) isFinished = true;
        else entityData = buffData.GetData();

        entityData.Speed.OriginValue += speed;
        entityData.ReadySpeed.OriginValue += readySpeed;

        // 수치에 영향이 없으면 이펙트 생성 X
        //if (entityData.Speed.IsOutInterval() || entityData.ReadySpeed.IsOutInterval()) return;

        BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
        if (effectPlayer != null)
        {
            m_effectPlayer = effectPlayer;
            effectPlayer.Init(caster.transform, 1000f);
            effectPlayer.PlayEffect();
        }
    }
}
