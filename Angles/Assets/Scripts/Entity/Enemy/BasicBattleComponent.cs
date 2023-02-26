using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBattleComponent : MonoBehaviour
{
    [SerializeField]
    protected SkillData normalSkillData;
    public SkillData NormalSkillData { get { return normalSkillData; } set { normalSkillData = value; } }

    [SerializeField]
    protected List<SkillData> loadSkillDatas = new List<SkillData>();

    protected virtual void UseSkill(SkillUseType skillUseType)
    {
        SkillData data;

        // 스킬을 사용할 수 있는 경우 스킬 사용
       if (NormalSkillData.CanUseSkill(skillUseType) == true) // 스킬을 사용할 수 없는 경우 기본 스킬을 사용하게 한다.
       {
           data = NormalSkillData.CopyData();

           loadSkillDatas.Add(data);
           UseSkillInList(skillUseType); // 리스트에 들어간 모든 오브젝트를 조건부로 실행해줌
       }
    }

    protected virtual void UseSkillInList(SkillUseType useType)
    {
        for (int i = 0; i < loadSkillDatas.Count; i++)
        {
            if (loadSkillDatas[i].CanUseSkill(useType) == false) continue;

            BasicSkill skill = GetSkillUsingName(loadSkillDatas[i].Name.ToString(), transform.position, Quaternion.identity);
            if (skill == null) continue;

            skill.PlayBasicSkill(transform);
            loadSkillDatas[i].UseSkill(loadSkillDatas);
        }
    }

    protected BasicSkill GetSkillUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        GameObject go = ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
        if (go == null) return null;

        BasicSkill skill = go.GetComponent<BasicSkill>();
        return skill;
    }
}
