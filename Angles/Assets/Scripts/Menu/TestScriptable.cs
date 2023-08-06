using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Scriptable<T> : ScriptableObject
{
    public abstract void Execute(T value);
}

[System.Serializable]
[CreateAssetMenu(fileName = "TestScriptable", menuName = "Scriptable Object/DamageMethod/TestScriptable", order = int.MaxValue)]
public class TestScriptable : Scriptable<RaycastHit2D>
{
    public override void Execute(RaycastHit2D value)
    {
        throw new System.NotImplementedException();
    }
}
