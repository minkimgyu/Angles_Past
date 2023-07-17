using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnFixedEffect", menuName = "Scriptable Object/EffectMethod/UnFixedEffect", order = int.MaxValue)]
public class UnFixedEffect : EffectMethod
{
    public override BasicEffectPlayer ReturnEffectFromPool()
    {
        BasicEffectPlayer effectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectName);
        if (effectPlayer == null) return null;

        effectPlayer.IsFixed = false;
        return effectPlayer;
    }
}
