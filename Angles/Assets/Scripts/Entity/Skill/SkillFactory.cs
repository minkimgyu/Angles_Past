using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSkillSO : BaseSO<BaseSkill> { }

public class SkillFactory : BaseFactory<BaseSkill>
{
    [SerializeField]
    StringBaseSkillSODictionary storedSkills;

    public override BaseSkill Order(GameObject caster, string name)
    {
        BaseSkill skill = storedSkills[name].Create();
        skill.Init(caster);

        return skill;
    }
}
