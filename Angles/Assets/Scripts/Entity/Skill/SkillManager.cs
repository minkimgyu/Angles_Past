using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CasterInfo
{
    public Transform m_caster;

    public string m_skillName;
    public string SkillName { get { return m_skillName; } }

    public CasterInfo(Transform caster, string name)
    {
        m_caster = caster;
        m_skillName = name;
    }
}

public struct DamageStat
{
    public string name; // --> 데이터 베이스에서 스킬 이름을 바탕(현 prefabData)으로 해당 데이터를 찾아서 넣어줌
}

public class SkillFactory
{
    Dictionary<string, ISkill> storedSkills;

    public void Init() // 이런 식으로 스킬 제작
    {
        // 이런식으로 다른 곳에서 돌려쓸 수 있도록 제작
        storedSkills.Add("CasterCircleRangeAttack", new CasterCircleRangeAttack(new LocationToContactor(), new FindInCircleRange(), new List<BaseMethod<RaycastHit2D[]>> { new DamageToRaycastHit() }));
    }

    public ISkill OrderSkill(Transform caster, SkillData data)
    {
        ISkill skill = storedSkills[data.PrefabName].CreateCopy();
        skill.Init(caster, data);

        return storedSkills[data.PrefabName];
    }
}

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager Instance { get { return instance; } }

    public SkillFactory skillFactory;

    public Dictionary<CasterInfo, ISkill> skills;

    private void Awake()
    {
        instance = this;

        skillFactory.Init();
    }

    public void AddSkillToList(Transform caster, SkillData data)
    {
        CasterInfo casterInfo = new CasterInfo(caster, data.PrefabName);
        skills.Add(casterInfo, skillFactory.OrderSkill(caster, data));
    }

    public void RemoveSkillInList(CasterInfo skillCaster)
    {
        if (skills[skillCaster] == null) return;

        skills.Remove(skillCaster);
    }

    void DoUpdate()
    {
        foreach (KeyValuePair<CasterInfo, ISkill> skill in skills)
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
