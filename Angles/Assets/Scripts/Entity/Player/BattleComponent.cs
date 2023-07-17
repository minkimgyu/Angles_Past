using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleComponent : MonoBehaviour
{
    [SerializeField]
    List<SkillCallData> m_possessingSkills; // 현재 보유하고 있는 스킬 --> 아이템 획득 시, 스킬을 추가해준다.

    public void UseSkill(SkillUseConditionType useType)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (!m_possessingSkills[i].CanUseSkill(useType)) continue;

            BasicSkill skill = ObjectPooler.SpawnFromPool<BasicSkill>(m_possessingSkills[i].Name, transform.position);
            if (skill == null) continue;

            skill.Execute(gameObject);


            if(m_possessingSkills[i].CanSubtractUseCount() && m_possessingSkills[i].IsUseCountZero())
            {
                RemoveSkillData((m_possessingSkills[i]));
            }
        }
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

        AddSkillData(skill);
        UseSkill(SkillUseConditionType.Get);
    }

    public void RemoveSkillData(SkillCallData skill) => m_possessingSkills.Remove(skill);

    public void AddSkillData(SkillCallData skill) => m_possessingSkills.Add(skill);
}
