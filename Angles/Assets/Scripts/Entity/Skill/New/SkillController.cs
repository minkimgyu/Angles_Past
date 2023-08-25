using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillController : MonoBehaviour
{
    List<BaseSkill> m_skills = new List<BaseSkill>(); // --> Ű ���� �ߺ��� �� �Ǽ� ����Ʈ�� ����ؾ��� ��

    SkillFactory skillFactory;

    Dictionary<BaseSkill.OverlapType, Action<BaseSkill>> OverlapTypeConditions;

    private void Awake()
    {
        skillFactory = FindObjectOfType<SkillFactory>();

        OverlapTypeConditions = new Dictionary<BaseSkill.OverlapType, Action<BaseSkill>>()
        {
            { BaseSkill.OverlapType.CountUp, (BaseSkill skill) => { skill.CountUp(); } },
            // ���� ��ų�� ã�Ƽ� ��� ī��Ʈ�� 1 �÷��ֱ�

            { BaseSkill.OverlapType.Restart, (BaseSkill skill) => { ResetSkill(skill.Name); } },
            // ���� ��ų�� ã�Ƽ� �ִٸ� Reset���ֱ�

            {BaseSkill.OverlapType.None, (BaseSkill skill) => { m_skills.Add(skill); } }
            // ���� ��ų�� �ϳ� �� �־��ش�.
        };
    }

    public void UseSkill(BaseSkill.UseConditionType useCondition)
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            if (m_skills[i].UseCondition != useCondition || m_skills[i].IsRunning == true) continue;

            m_skills[i].Init(gameObject);
            m_skills[i].Execute(); // ���� ���ư��� �ʴ� ��ų�� �����


            if (m_skills[i].IsUseCountZero()) m_skills.Remove(m_skills[i]);
        }
    }

    void ResetSkill(string name)
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            if (m_skills[i].Name == name && m_skills[i].IsRunning == true)
            {
                m_skills[i].Reset();
            }
        }
    }

    public void AddSkillToList(string name)
    {
        BaseSkill skill = skillFactory.Order(gameObject, name);
        OverlapTypeConditions[skill.OverlapCondition](skill); 

        UseSkill(BaseSkill.UseConditionType.Get);
    }

    public void RemoveSkillInList(string name)
    {
        BaseSkill skill = m_skills.Find(x => x.Name == name);
        if (skill == null) return;

        m_skills.Remove(skill); // --> ��ų ��� ����
    }

    void DoUpdate()
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            if (m_skills[i].NowFinish == true)
            {
                m_skills[i].End();
                m_skills.Remove(m_skills[i]);
                continue;
            }

            m_skills[i].Execute();
        }
    }

    private void Update()
    {
        DoUpdate();
    }
}
