using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntitySpeedBuff : PassiveBuff
{
    PlayerData entityData;

    [SerializeField]
    float speed;

    float offset;

    public override void OnEnd()
    {
        entityData.Speed -= speed;

        entityData.Speed += offset; // 최대 최소 적용
    }

    public override void OnStart(GameObject caster)
    {
        caster.TryGetComponent(out IBuff<PlayerData> buffData);

        if (buffData == null) isFinished = true;
        else entityData = buffData.GetData(); 

        entityData.Speed += speed;

        if(entityData.Speed > entityData.MaxSpeed)
        {
            offset = entityData.Speed - entityData.MaxSpeed;
            entityData.Speed = entityData.MaxSpeed;
        }
        else if(entityData.Speed < entityData.MinSpeed)
        {
            offset = entityData.MinSpeed - entityData.Speed;
            entityData.Speed = entityData.MinSpeed;
        }
    }
}
