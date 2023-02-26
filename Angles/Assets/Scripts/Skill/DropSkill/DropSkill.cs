using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSkill : MonoBehaviour
{
    [SerializeField]
    SkillData dropSkillData;
    public SkillData DropSkillData
    {
        get
        {
            return dropSkillData;
        }
        set
        {
            dropSkillData = value;
        }
    }
}