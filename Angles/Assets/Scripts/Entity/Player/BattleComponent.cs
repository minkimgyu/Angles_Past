using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleComponent : MonoBehaviour
{
    [SerializeField]
    List<SkillCallData> m_possessingSkills; // ���� �����ϰ� �ִ� ��ų --> ������ ȹ�� ��, ��ų�� �߰����ش�.

    [SerializeField]
    List<BasicSkill> m_activeSkills; // ���� ��Ƽ�� �Ǿ��ִ� ��ų 

    public void UseSkill(SkillUseConditionType useType)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].CanUseSkill(useType) == false) continue;

            if (CheckActiveSkillAndExecute(m_possessingSkills[i]) == true) continue;


             BasicSkill skill = ObjectPooler.SpawnFromPool<BasicSkill>(m_possessingSkills[i].Name, transform.position);
            if (skill == null) continue;


            m_activeSkills.Add(skill);

            skill.RemoveFromList += RemoveFromActiveSkills;
            skill.Execute(gameObject);

            if (m_possessingSkills[i].CanSubtractUseCount() && m_possessingSkills[i].IsUseCountZero())
            {
                m_possessingSkills.Remove(m_possessingSkills[i]);
            }
        }
    }

    bool CheckActiveSkillAndExecute(SkillCallData callData)
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

    public void RemoveFromActiveSkills(BasicSkill skill)
    {
        skill.RemoveFromList -= RemoveFromActiveSkills;
        m_activeSkills.Remove(skill);
    }

    public void LootingSkill(SkillCallData skill)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].Name == skill.Name)
            {
                m_possessingSkills[i].UpUseCount();
                return;
            }
        }

        m_possessingSkills.Add(skill);
        UseSkill(SkillUseConditionType.Get);
    }
}
