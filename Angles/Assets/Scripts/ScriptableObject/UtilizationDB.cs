using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilizationDB", menuName = "Scriptable Object/DB/UtilizationDB")]
public class UtilizationDB : ScriptableObject
{
    [SerializeField]
    List<SkillCallData> skillCallDatas;
    public List<SkillCallData> SkillCallDatas { get { return skillCallDatas; } }

    [SerializeField]
    List<BasicSkill> skillDatas;
    public List<BasicSkill> SkillDatas { get { return skillDatas; } }

    [SerializeField]
    List<BuffData> buffDatas;
    public List<BuffData> BuffDatas { get { return buffDatas; } }
}