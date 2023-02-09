using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class Player : Entity
{
    SkillType skill;
    public SkillType Skill
    {
        get
        {
            return skill;
        }
        set
        {
            skill = value;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public void ResetSkill(SkillType mode)
    {
        skill = mode;
    }
}
