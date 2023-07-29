using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleComponent : MonoBehaviour
{
    [SerializeField]
    List<SkillData> m_possessingSkills; // ���� �����ϰ� �ִ� ��ų --> ������ ȹ�� ��, ��ų�� �߰����ش�.
    public List<SkillData> PossessingSkills { get { return m_possessingSkills; } }

    [SerializeField]
    List<BasicSkill> m_activeSkills; // ���� ��Ƽ�� �Ǿ��ִ� ��ų 

    [SerializeField]
    SkillSynthesisComponent m_synthesisComponent; // ��ų �ռ� ������Ʈ

    public void UseSkill(SkillUseConditionType useType)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].CanUseSkill(useType) == false) continue;

            if (CheckActiveSkillAndExecute(m_possessingSkills[i]) == true) continue;


            BasicSkill skill = ObjectPooler.SpawnFromPool<BasicSkill>(m_possessingSkills[i].PrefabName, transform.position);
            if (skill == null) continue;


            m_activeSkills.Add(skill);

            skill.Init(this, m_possessingSkills[i]);
            skill.Execute(gameObject);

            if (m_possessingSkills[i].CanSubtractUseCount() && m_possessingSkills[i].IsUseCountZero())
            {
                m_possessingSkills.Remove(m_possessingSkills[i]);
            }
        }
    }

    public void RemoveSkillFromPossessingSkills(SkillData data)
    {
        SkillData skillData = m_possessingSkills.Find(x => x.Name == data.Name);
        if (skillData == null) return;

        m_possessingSkills.Remove(skillData);
    }

    public void RemoveFromActiveSkills(BasicSkill skill)
    {
        m_activeSkills.Remove(skill);
    }

    bool CheckActiveSkillAndExecute(SkillData callData)
    {
        if (callData.OverlapType == SkillOverlapType.None) return false;

        BasicSkill loadedSkill = m_activeSkills.Find(x => x.Data.Name == callData.Name);

        if (loadedSkill != null)
        {
            loadedSkill.Execute(gameObject);
            m_possessingSkills.Remove(callData);

            return true;
        }

        return false;
    }

    public void LootingSkill(SkillData skill)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].Name == skill.Name)
            {
                m_possessingSkills[i].UpUseCount();
                return;
            }
        }

        SkillData synthesisedData = m_synthesisComponent.SynthesisSkill(PossessingSkills, skill);
        if(synthesisedData == null)
        {
            m_possessingSkills.Add(skill);
        }
        else
        {
            m_possessingSkills.Add(synthesisedData);
        }
        
        UseSkill(SkillUseConditionType.Get);
    }
}
