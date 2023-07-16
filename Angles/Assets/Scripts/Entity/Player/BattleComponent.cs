using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public class SkillSupportData // �̷������� ��ų�� �ʿ��� ������ ��� ������
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
    List<SkillCallData> m_possessingSkills; // ���� �����ϰ� �ִ� ��ų --> ������ ȹ�� ��, ��ų�� �߰����ش�.

    public Action<SkillUseType> OnUsingSkill;

    private void Awake()    
    {
        //OnUsingSkill += UseSkill; // attack state Enter�� ���
        //OnUsingSkill -= UseSkill; // attack state Exit�� ���
    }

    public void UseSkill(SkillUseType useType)
    {
        //SkillSupportData supportData = ReturnSkillSupportData();   

        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (!m_possessingSkills[i].CallCondition.Check(useType)) continue;

            BasicSkill skill = GetSkillUsingName(m_possessingSkills[i].Name, transform.position);
            if (skill == null) continue;

            skill.Execute(this);

            //skill.Execute(supportData);
            //m_possessingSkills[i].AfterSkillAdjustment(m_possessingSkills);
        }
    }

    void LootingSkill(SkillCallData skill)
    {
        for (int i = 0; i < m_possessingSkills.Count; i++)
        {
            if (m_possessingSkills[i].Name == skill.Name)
            {
                //m_possessingSkills[i].Loot(this); 
                return;
            }
        }

        AddSkillData(skill);
        OnUsingSkill(SkillUseType.Get);
    }

    public void RemoveSkillData(SkillCallData skill) => m_possessingSkills.Remove(skill);

    void AddSkillData(SkillCallData skill) => m_possessingSkills.Add(skill);


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

    private BasicSkill GetSkillUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        GameObject go = ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
        return go.GetComponent<BasicSkill>();
    }

    private BasicSkill GetSkillUsingName(string name, Vector3 pos) // --> �����ͺ��̽����� ��ũ���ͺ� ������Ʈ �޾ƿ��� ������� ��ȯ
    {
        GameObject go = ObjectPooler.SpawnFromPool(name, pos, Quaternion.identity, null);
        return go.GetComponent<BasicSkill>();
    }
}
