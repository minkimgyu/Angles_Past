using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʈ Ǯ������ �ش� �̸��� ����Ʈ�� �����´�.
/// </summary>
abstract public class EffectMethod : ScriptableObject
{
    [SerializeField]
    protected string effectName;

    public abstract BasicEffectPlayer ReturnEffectFromPool();
}