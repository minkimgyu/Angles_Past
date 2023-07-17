using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PositionMethod : ScriptableObject
{
    public abstract void Init(Transform caster, BasicSkill me);

    public abstract void DoUpdate(BasicSkill me);
}
