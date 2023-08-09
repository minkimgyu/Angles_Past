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
        skill.Execute();

        return storedSkills[data.PrefabName];
    }
}

public class SkillManager : MonoBehaviour
{
    public SkillFactory skillFactory;

    public Dictionary<CasterInfo, ISkill> skills;

    [SerializeField]
    SkillCueEventSO skillAddCueEventSo;

    [SerializeField]
    SkillCueEventSO skillRemoveCueEventSo;

    private void Awake()
    {
        skillFactory.Init();
        skillAddCueEventSo.OnSkillCueRequested += AddSkillToList;
        skillRemoveCueEventSo.OnSkillCueRequested += RemoveSkillInList;
    }

    private void OnDisable()
    {
        skillAddCueEventSo.OnSkillCueRequested -= AddSkillToList;
        skillRemoveCueEventSo.OnSkillCueRequested -= RemoveSkillInList;
    }

    public void AddSkillToList(Transform caster, SkillData skillData)
    {
        CasterInfo casterInfo = new CasterInfo(caster, skillData.PrefabName);

        if (skillData.OverlapType != SkillOverlapType.None && skills[casterInfo] != null) 
        {
            // 만약 동일 케스터가 이전에 사용한 스킬이 존재한다면?
            OverlapSkillInList(casterInfo); // 스킬 재사용 ㄱㄱ
        }
        else
        {
            skills.Add(casterInfo, skillFactory.OrderSkill(caster, skillData));
        }
    }

    public void RemoveSkillInList(Transform caster, SkillData skillData)
    {
        CasterInfo casterInfo = new CasterInfo(caster, skillData.PrefabName);
        if (skills[casterInfo] == null) return;

        skills[casterInfo].Finish(); // --> 스킬 사용 중지
    }

    public void OverlapSkillInList(CasterInfo casterInfo)
    {
        skills[casterInfo].Reset();
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
