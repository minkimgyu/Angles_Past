using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected EntityTag inheritedTag;
    public EntityTag InheritedTag
    {
        get { return inheritedTag; }
    }

    public virtual void InitData() { }
}
