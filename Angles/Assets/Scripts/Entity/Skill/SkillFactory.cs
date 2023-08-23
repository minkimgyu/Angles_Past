using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSkillSO : BaseSO<BaseSkill> { }


public class SkillFactory : MonoBehaviour
{
    [SerializeField]
    StringBaseSkillSODictionary storedSkills;

    public BaseSkill OrderSkill(GameObject caster, string name)
    {
        BaseSkill skill = storedSkills[name].Create();
        skill.Init(caster);

        return skill;
    }
}
