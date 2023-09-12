using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillController : MonoBehaviour
{
    [SerializeReference]
    List<BaseSkill> m_skills = new List<BaseSkill>(); // --> 키 값이 중복이 안 되서 리스트로 사용해야할 듯

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
            // 같은 스킬을 찾아서 사용 카운트를 1 올려주기

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
            // 같은 스킬을 찾아서 있다면 Reset해주기

            { BaseSkill.OverlapType.Respawn, (BaseSkill skill) =>
                {
                    ActionInSkillList(skill.Name, skill, (BaseSkill tmpSkill) =>
                    {
                        tmpSkill.Execute(); // 스킬 재 실행
                        canOverlap = true;
                    });
                }
            },

            {BaseSkill.OverlapType.None, (BaseSkill skill) => { m_skills.Add(skill); } }
            // 같은 스킬을 하나 더 넣어준다.
        };
    }

    public void UseSkill(BaseSkill.UseConditionType useCondition)
    {
        for (int i = 0; i < m_skills.Count; i++)
        {
            if (m_skills[i].UseCondition != useCondition || m_skills[i].IsRunning == true || m_skills[i].CantUseAgain == true) continue;
            // None이 아닌 경우는 스킬을 useCondition에 맞게 실행하는 것이 아닌 다른 방법으로 재실행하는 방법으로 처리해줘야함

            m_skills[i].Init(gameObject);
            m_skills[i].Execute(); // 현재 돌아가지 않는 스킬을 사용함
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

        m_skills.Remove(skill); // --> 스킬 사용 중지
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
