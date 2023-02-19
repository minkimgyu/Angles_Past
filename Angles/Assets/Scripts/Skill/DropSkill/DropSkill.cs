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
    int count = 1;
    public int Count { get { return count; } set { count = value; } }

    public SkillData(SkillName skillName, SkillUseType skillType, int skillUseCount = 1)
    {
        name = skillName;
        type = skillType;
        count = skillUseCount;
    }

    public bool CanUseSkill(SkillUseType skillType)
    {
        return Name != SkillName.None && Type == skillType && Count >= 1;
    }

    public void UseSkill(List<SkillData> skillDatas)
    {
        if (CanUseSkill(type) == false) return;

        count -= 1;
        if(count <= 0) skillDatas.Remove(this);
    }

    public SkillData CopyData()
    {
        return new SkillData(name, type, count);
    }

    public void ResetSkill()
    {
        Name = SkillName.None;
        Type = SkillUseType.None;
        count = 0;
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
