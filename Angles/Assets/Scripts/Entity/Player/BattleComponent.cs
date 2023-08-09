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
    SkillCueEventSO skillAddCueEventSo;

    [SerializeField]
    SkillCueEventSO skillRemoveCueEventSo;

    public void UseSkill(SkillUseConditionType useType)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].CanUseSkill(useType) == false) continue;

            skillAddCueEventSo.OnSkillCueRequested(transform, m_possessingSkills[i]);

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

    public void RemoveFromActiveSkill(Transform caster, SkillData skillData)
    {
        skillRemoveCueEventSo.OnSkillCueRequested(caster, skillData); // ��ų ��� ����
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
        
        UseSkill(SkillUseConditionType.Get);
    }
}
