using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : UnitaskUtility
{
    [SerializeField]
    SkillName skillName;
    public SkillName SkillName { get { return skillName; } set { skillName = value; } }

    [SerializeField]
    SkillUseType skillUseType;
    public SkillUseType SkillUseType { get { return skillUseType; } set { skillUseType = value; } }

    protected BattleComponent loadBattle;

    protected BasicEffect effect;

    protected virtual void Awake()
    {
        effect = GetComponent<BasicEffect>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ObjectPooler.ReturnToPool(gameObject);
    }

    protected GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    }

    public virtual void PlaySkill(SkillSupportData suportDatas)
    {
        
    }
}
