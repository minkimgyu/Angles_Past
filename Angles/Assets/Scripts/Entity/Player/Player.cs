using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class Player : Entity
{
    [SerializeField]
    SkillData normalSkillData;
    public SkillData NormalSkillData
    {
        get
        {
            return normalSkillData;
        }
        set
        {
            normalSkillData = value;
        }
    }

    [SerializeField]
    SkillData skillData;
    public SkillData SkillData
    {
        get
        {
            return skillData;
        }
        set
        {
            skillData = value;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
