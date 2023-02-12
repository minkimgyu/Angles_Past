using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    [SerializeField]
    SkillName name;
    public SkillName Name { get { return name; } set { name = value; } }

    [SerializeField]
    SkillUseType type;
    public SkillUseType Type { get { return type; } set { type = value; } }

    [SerializeField]
    int skillUseCount = 1;

    public bool CanUseSkill(SkillUseType type)
    {
        return Name != SkillName.None && Type == type;
    }

    public void ResetSkill()
    {
        Name = SkillName.None;
        Type = SkillUseType.None;
    }
}

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
