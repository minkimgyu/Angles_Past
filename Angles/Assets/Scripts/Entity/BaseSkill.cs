using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill<T> where T : class
{
    void Init(T value);

    void Execute(T value);
}

public class BaseSkill : MonoBehaviour, ISkill<SkillSupportData>
{
    SkillData skillData;
    public SkillData SkillData
    {
        get { return skillData; }
        set { skillData = value; }
    }

    public void Init(SkillSupportData value)
    {
        throw new System.NotImplementedException();
    }




    public void Execute(SkillSupportData value)
    {
        Init(value);
    }

    protected GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    }
}
