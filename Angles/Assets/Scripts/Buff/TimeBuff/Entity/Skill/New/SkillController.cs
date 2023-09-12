using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillController : MonoBehaviour
{
    [SerializeReference]
    List<BaseSkill> m_skills = new List<BaseSkill>(); // --> Ű ���� �ߺ��� �� �Ǽ� ����Ʈ�� ����ؾ��� ��

    Dictionary<BaseSkill.OverlapType, Action<BaseSkill>> OverlapTypeConditions;
    bool canOverlap = false;

    private void Awake()
    {
        OverlapTypeConditions = new Dictionary<BaseSkill.OverlapType, Action<BaseSkill>>()
        {
            { BaseSkill.OverlapType.CountUp, (BaseSkill skill) =>
                {
                    ActionInSkillList(skill.Name, skill, (BaseSkill tmpSkill) =>
                    {
                        tmpSkill.CountUp();
                        canOverlap = true;
                    });
                }
            },
            // ���� ��ų�� ã�Ƽ� ��� ī��Ʈ�� 1 �÷��ֱ�

            { BaseSkill.OverlapType.Restart, (BaseSkill skill) => 
                {
                    ActionInSkillList(skill.Name, skill, (BaseSkill tmpSkill) =>
                    {
                        if(tmpSkill.IsRunning == true)
                        {
                            tmpSkill.Reset();
                            canOverlap = true;
                        }
                    });
                } 
            },
            // ���� ��ų�� ã�Ƽ� �ִٸ� Reset���ֱ�

            { BaseSkill.OverlapType.Respawn, (BaseSkill skill) =>
                {
                    ActionInSkillList(skill.Name, skill, (BaseSkill tmpSkill) =>
                    {
                        tmpSkill.Execute(); // ��ų �� ����
                        canOverlap = true;
                    });
                }
            },

            {BaseSkill.OverlapType.None, (BaseSkill skill) => { m_skills.Add(skill); } }
            // ���� ��ų�� �ϳ� �� �־��ش�.
        };
    }

    public void UseSkill(BaseSkill.UseConditionType useCondition)
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            if (m_skills[i].UseCondition != useCondition || m_skills[i].IsRunning == true || m_skills[i].CantUseAgain == true) continue;
            // None�� �ƴ� ���� ��ų�� useCondition�� �°� �����ϴ� ���� �ƴ� �ٸ� ������� ������ϴ� ������� ó���������

            m_skills[i].Init(gameObject);
            m_skills[i].Execute(); // ���� ���ư��� �ʴ� ��ų�� �����
        }
    }

    void ActionInSkillList(string name, BaseSkill skill, Action<BaseSkill> action)
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            if (m_skills[i].Name == name)
            {
                action(m_skills[i]);
                return;
            }
        }

        m_skills.Add(skill);
    }

    public void AddSkillToList(string name)
    {
        BaseSkill skill = SkillFactory.Order(name);
        OverlapTypeConditions[skill.OverlapCondition](skill);

        if(canOverlap == false) UseSkill(BaseSkill.UseConditionType.Get);
        else canOverlap = false;
    }

    public void RemoveSkillInList(string name)
    {
        BaseSkill skill = m_skills.Find(x => x.Name == name);
        if (skill == null) return;

        m_skills.Remove(skill); // --> ��ų ��� ����
    }

    public void RemoveAllSkillInList()
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            m_skills[i].StopPredelayEffect();

            m_skills[i].End();
            m_skills.Remove(m_skills[i]);
            i--;
        }
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

            if(m_skills[i].IsRunning)
                m_skills[i].Execute();
        }
    }

    private void Update()
    {
        DoUpdate();
    }
}
