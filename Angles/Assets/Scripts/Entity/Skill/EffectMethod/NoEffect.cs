using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NoEffect", menuName = "Scriptable Object/EffectMethod/NoEffect", order = int.MaxValue)]
public class NoEffect : EffectMethod
{
    public override BasicEffectPlayer ReturnEffectFromPool()
    {
        return null;
    }
}
