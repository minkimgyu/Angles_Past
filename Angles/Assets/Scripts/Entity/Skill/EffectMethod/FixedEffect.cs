using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "FixedEffect", menuName = "Scriptable Object/EffectMethod/FixedEffect", order = int.MaxValue)]
public class FixedEffect : EffectMethod
{
    public override BasicEffectPlayer ReturnEffectFromPool()
    {
        BasicEffectPlayer effectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectName);
        if (effectPlayer == null) return null;

        effectPlayer.IsFixed = true;
        return effectPlayer;
    }
}
