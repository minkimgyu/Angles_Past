using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSkillSO : BaseSO<BaseSkill> { }

public class SkillFactory : MonoBehaviour
{
    [SerializeField]
    StringBaseSkillSODictionary storedSkills;

    static SkillFactory inst;
    private void Awake() => inst = this;

    public static BaseSkill Order(string name)
    {
        BaseSkill skill = inst.storedSkills[name].Create();
        return skill;
    }
}
