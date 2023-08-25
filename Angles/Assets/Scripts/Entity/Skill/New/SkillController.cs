using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillController : MonoBehaviour
{
    List<BaseSkill> m_skills = new List<BaseSkill>(); // --> 키 값이 중복이 안 되서 리스트로 사용해야할 듯

    SkillFactory skillFactory;

    Dictionary<BaseSkill.OverlapType, Action<BaseSkill>> OverlapTypeConditions;

    private void Awake()
    {
        skillFactory = FindObjectOfType<SkillFactory>();

        OverlapTypeConditions = new Dictionary<BaseSkill.OverlapType, Action<BaseSkill>>()
        {
            { BaseSkill.OverlapType.CountUp, (BaseSkill skill) => { skill.CountUp(); } },
            // 같은 스킬을 찾아서 사용 카운트를 1 올려주기

            { BaseSkill.OverlapType.Restart, (BaseSkill skill) => { ResetSkill(skill.Name); } },
            // 같은 스킬을 찾아서 있다면 Reset해주기

            {BaseSkill.OverlapType.None, (BaseSkill skill) => { m_skills.Add(skill); } }
            // 같은 스킬을 하나 더 넣어준다.
        };
    }

    public void UseSkill(BaseSkill.UseConditionType useCondition)
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            if (m_skills[i].UseCondition != useCondition || m_skills[i].IsRunning == true) continue;

            m_skills[i].Init(gameObject);
            m_skills[i].Execute(); // 현재 돌아가지 않는 스킬을 사용함


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

        m_skills.Remove(skill); // --> 스킬 사용 중지
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
