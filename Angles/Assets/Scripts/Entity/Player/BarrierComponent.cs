using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierComponent : MonoBehaviour
{
    [SerializeField]
    EffectMethod effectMethod;

    BasicEffectPlayer effectPlayer;

    [SerializeField]
    int barrierCount = 0;

    [SerializeField]
    int maxBarrierCount = 1;

    public void AddBarrier()
    {
        if (maxBarrierCount <= barrierCount) return;

        barrierCount += 1;
        effectPlayer = effectMethod.ReturnEffectFromPool();
        effectPlayer.Init(transform, 10000000000000f);
        effectPlayer.PlayEffect();
    }

    public void RemoveBarrier()
    {
        effectPlayer.StopEffect();
        effectPlayer = null;
    }

    public bool CanBarrierAbsorb()
    {
        if (barrierCount <= 0) return false;

        barrierCount -= 1;

        if (barrierCount <= 0)
        {
            barrierCount = 0;
            RemoveBarrier();
        }

        return true;
    }
}
