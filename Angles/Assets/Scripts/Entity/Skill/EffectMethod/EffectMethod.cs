using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀링에서 해당 이름의 이펙트를 꺼내온다.
/// </summary>
abstract public class EffectMethod : ScriptableObject
{
    [SerializeField]
    protected string effectName;

    public abstract BasicEffectPlayer ReturnEffectFromPool();
}