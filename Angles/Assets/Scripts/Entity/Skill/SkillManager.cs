using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillCaster
{
    public GameObject m_caster;
    public string m_skillName;

    public SkillCaster(GameObject caster, string name)
    {
        m_caster = caster;
        m_skillName = name;
    }
}

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager Instance { get { return instance; } }

    public Dictionary<SkillCaster, ISkill> skills;

    private void Awake()
    {
        instance = this;
    }

    public void AddSkillToList(SkillCaster skillCaster, ISkill skill)
    {
        skills.Add(skillCaster, skill);
    }

    public void RemoveSkillInList(SkillCaster skillCaster)
    {
        if (skills[skillCaster] == null) return;

        skills.Remove(skillCaster);
    }

    void DoUpdate()
    {
        foreach (KeyValuePair<SkillCaster, ISkill> skill in skills)
        {
            if (skill.Value.CheckIsFinish() == false)
            {
                skill.Value.End();
                skills.Remove(skill.Key);
                continue;
            }

            skill.Value.Execute();
        }
    }

    private void Update()
    {
        DoUpdate();
    }
}
