using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    //[SerializeField] 
    //protected EntityTag inheritedTag;
    public EntityTag InheritedTag { get { return (EntityTag)Enum.Parse(typeof(EntityTag), gameObject.tag); } }

    public void Initialize() { }
}
   