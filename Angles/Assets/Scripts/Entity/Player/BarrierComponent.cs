using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierComponent : MonoBehaviour
{
    BasicEffectPlayer effectPlayer;

    [SerializeField]
    int barrierCount = 0;

    [SerializeField]
    int maxBarrierCount = 1;

    public void AddBarrier()
    {
        if (maxBarrierCount <= barrierCount) return;
        
        SoundManager.Instance.PlaySFX(transform.position, "SpawnBall", 0.05f);
        barrierCount += 1;
        effectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>("Barrier");
        effectPlayer.Init(transform);
        effectPlayer.PlayEffect();
    }

    public void RemoveBarrier(float damage)
    {
        if (damage == 0f) return;

        SoundManager.Instance.PlaySFX(transform.position, "BarrierCrack", 0.3f);
        effectPlayer.StopEffect();
        effectPlayer = null;
    }

    public bool CanBarrierAbsorb(float damage)
    {
        if (barrierCount <= 0) return false;

        barrierCount -= 1;

        if (barrierCount <= 0)
        {
            barrierCount = 0;
            RemoveBarrier(damage);
        }

        return true;
    }
}
