using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public class SkillSupportData // 이런식으로 스킬에 필요한 데이터 묶어서 보내기
//{
//    public List<Vector3> contactPos;
//    public List<Entity> contactEntity;

//    public SkillSupportData(List<Entity> contactEntity, List<Vector3> contactPos)
//    {
//        this.contactEntity = contactEntity;
//        this.contactPos = contactPos;
//    }
//}

//[System.Serializable]
//public struct ContactData
//{
//    public GameObject go;
//    public Vector3 pos;

//    public ContactData(GameObject go, Vector3 pos)
//    {
//        this.go = go;
//        this.pos = pos;
//    }
//}

public class BattleComponent : TagCheckComponent
{
    //public List<ContactData> m_contactDatas = new List<ContactData>();

    [SerializeField]
    List<SkillData> m_possessingSkills; // 현재 보유하고 있는 스킬 --> 아이템 획득 시, 스킬을 추가해준다.

    public Action<SkillUseType> OnUsingSkill;

    private void Awake()    
    {
        //OnUsingSkill += UseSkill; // attack state Enter의 경우
        //OnUsingSkill -= UseSkill; // attack state Exit의 경우
    }

    public void UseSkill(SkillUseType useType)
    {
        //SkillSupportData supportData = ReturnSkillSupportData();

        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].UseType != useType) continue;

            BaseSkill skill = GetSkillUsingName(m_possessingSkills[i].Name, transform.position);
            if (skill == null) continue;
            //skill.Execute(supportData);

            m_possessingSkills[i].AfterSkillAdjustment(m_possessingSkills);
        }
    }

    void LootingSkill(SkillData data)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].Name != data.Name) continue;
            m_possessingSkills[i].CountCheckBySynthesis(data.Synthesis);
        }

        AddSkillData(data);
        OnUsingSkill(SkillUseType.Get);
    }

    void RemoveSkillData(SkillData data) => m_possessingSkills.Remove(data);

    void AddSkillData(SkillData data) => m_possessingSkills.Add(data);


    //public void CallWhenCollisionEnter(Collision2D col)
    //{
    //    ContactData contactData = new ContactData(col.gameObject, col.contacts[0].point);
    //    m_contactDatas.Add(contactData);
    //    OnUsingSkill(SkillUseType.Contact);
    //}

    //public void CallWhenCollisionExit(Collision2D col)
    //{
    //    for (int i = 0; i < m_contactDatas.Count; i++)
    //    {
    //        if (m_contactDatas[i].go == col.gameObject)
    //        {
    //            m_contactDatas.RemoveAt(i);
    //            return;
    //        }
    //    }
    //}

    //SkillSupportData ReturnSkillSupportData()
    //{
    //    List<Vector3> pos = new List<Vector3>();
    //    List<Entity> entity = new List<Entity>();

    //    for (int i = 0; i < m_contactDatas.Count; i++)
    //    {
    //       pos.Add(m_contactDatas[i].pos);
    //       entity.Add(m_contactDatas[i].go.GetComponent<Entity>());
    //    }

    //    SkillSupportData supportData = new SkillSupportData(entity, pos);
    //    return supportData;
    //}

    private BaseSkill GetSkillUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        GameObject go = ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
        return go.GetComponent<BaseSkill>();
    }

    private BaseSkill GetSkillUsingName(string name, Vector3 pos)
    {
        GameObject go = ObjectPooler.SpawnFromPool(name, pos, Quaternion.identity, null);
        return go.GetComponent<BaseSkill>();
    }
}
