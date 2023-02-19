using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillControllComponent : MonoBehaviour
{
    //Player player;
    //public List<BasicSkill> haveSkill = new List<BasicSkill>();

    //public List<SkillData> loadSkillData = new List<SkillData>();
    //public void RemoveSkill(SkillData skill) => loadSkillData.Remove(skill);
    //public void AddSkill(SkillData skill) => loadSkillData.Add(skill);

    //private void Start()
    //{
    //    player = GetComponent<Player>();
    //    InitSkill();
    //}

    //void InitSkill()
    //{
    //    foreach(SkillName skillName in SkillName.GetValues(typeof(SkillName)))
    //    {
    //        string name = skillName.ToString();
    //        if (name == "None") continue;

    //        AddSkillToList("Prefabs/" + "Skill/" + name);
    //    }
    //}

    //void AddSkillToList(string path)
    //{
    //    GameObject go = Resources.Load(path) as GameObject;
    //    haveSkill.Add(Instantiate(go).GetComponent<BasicSkill>());
    //}

    //public void UseSkillInList(SkillUseType useType, List<Collision2D> entitys)
    //{
    //    for (int i = 0; i < loadSkillData.Count; i++)
    //    {
    //        BasicSkill skill = haveSkill.Find(x => x.SkillData.Name == loadSkillData[i].Name);
    //        skill.PlaySkill();

    //        loadSkillData[i].UseSkill(loadSkillData);
    //    }
    //}

}
