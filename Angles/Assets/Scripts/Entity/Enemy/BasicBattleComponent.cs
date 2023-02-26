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

        // ��ų�� ����� �� �ִ� ��� ��ų ���
       if (NormalSkillData.CanUseSkill(skillUseType) == true) // ��ų�� ����� �� ���� ��� �⺻ ��ų�� ����ϰ� �Ѵ�.
       {
           data = NormalSkillData.CopyData();

           loadSkillDatas.Add(data);
           UseSkillInList(skillUseType); // ����Ʈ�� �� ��� ������Ʈ�� ���Ǻη� ��������
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
