using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityData
{
    [SerializeField]
    protected string name;
    public string Name { get { return name; } set { name = value; } }

    [SerializeField]
    protected float hp;
    public float Hp { get { return Hp; } set { hp = value; } }

    [SerializeField]
    protected float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [SerializeField]
    protected float stunTime;
    public float StunTime { get { return stunTime; } set { stunTime = value; } }
}

[System.Serializable]
public class PlayerData : EntityData
{
    [SerializeField]
    float readySpeed = 5;
    public float ReadySpeed { get{ return readySpeed; } set { readySpeed = value; } }

    [SerializeField]
    float minSpeedRatio;
    public float MinSpeedRatio { get { return minSpeedRatio; } set { minSpeedRatio = value; } }

    [SerializeField]
    float maxSpeedRatio;
    public float MaxSpeedRatio { get { return maxSpeedRatio; } set { maxSpeedRatio = value; } }

    [SerializeField]
    float attackThrust;
    public float AttackThrust { get { return attackThrust; } set { attackThrust = value; } }

    [SerializeField]
    float attackTime;
    public float AttackTime { get { return attackTime; } set { attackTime = value; } }

    [SerializeField]
    float attackCancelOffset;
    public float AttackCancelOffset { get { return attackCancelOffset; } set { attackCancelOffset = value; } }


    [SerializeField]
    float reflectAttackThrust;
    public float ReflectAttackThrust { get { return reflectAttackThrust; } set { reflectAttackThrust = value; } }


    [SerializeField]
    float maxDashCount;
    public float MaxDashCount { get { return maxDashCount; } set { maxDashCount = value; } }


    [SerializeField]
    float dashTime;
    public float DashTime { get { return dashTime; } set { dashTime = value; } }


    [SerializeField]
    float dashThrust;
    public float DashThrust { get { return dashThrust; } set { dashThrust = value; } }


    [SerializeField]
    float dashRatio;
    public float DashRatio { get { return dashRatio; } set { dashRatio = value; } }
}

[System.Serializable]
public class EnemyData : EntityData
{
    [SerializeField]
    float skillUseRange;
    public float SkillUseRange { get { return skillUseRange; } set { skillUseRange = value; } }

    public EnemyData CopyData()
    {
        EnemyData skillData = new EnemyData();
        return skillData;
    }
}

[System.Serializable]
public class SkillData
{
    [SerializeField]
    SkillName name;
    public SkillName Name { get { return name; } set { name = value; } }

    [SerializeField]
    SkillUseType useType;
    public SkillUseType UseType { get { return useType; } set { useType = value; } }

    [SerializeField]
    int useCount = 1;
    public int UseCount { get { return useCount; } set { useCount = value; } }

    [SerializeField]
    float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    [SerializeField]
    float disableTime;
    public float DisableTime { get { return disableTime; } set { disableTime = value; } }

    [SerializeField]
    EntityTag[] hitTarget;
    public EntityTag[] HitTarget { get { return hitTarget; } set { hitTarget = value; } }

    #region Fn

    public bool CanUseSkill(SkillUseType skillType)
    {
        return name != SkillName.None && useType == skillType && UseCount >= 1;
    }

    public void UseSkill(List<SkillData> skillDatas)
    {
        if (CanUseSkill(useType) == false) return;

        useCount -= 1;
        if (useCount <= 0) skillDatas.Remove(this);
    }


    public SkillData()
    {
        
    }

    public SkillData(SkillName name, SkillUseType useType, int useCount, float damage, float disableTime, EntityTag[] hitTarget)
    {
        this.name = name;
        this.useType = useType;
        this.useCount = useCount;
        this.damage = damage;
        this.disableTime = disableTime;
        this.hitTarget = hitTarget;
    }

    public SkillData CopyData()
    {
        SkillData skillData = new SkillData(name, useType, useCount, damage, disableTime, hitTarget);
        return skillData;
    }

    public void ResetSkill()
    {
        name = SkillName.None;
        useType = SkillUseType.None;
        useCount = 0;
    }

    #endregion
}

[CreateAssetMenu(fileName = "EntityDB", menuName = "Scriptable Object/EntityDB")]
public class EntityDB : ScriptableObject
{
    public PlayerData Player;
    public List<EnemyData> Enemy;
    public List<SkillData> Skill;

    public void ResetData()
    {
        Player = new PlayerData();
        Enemy = new List<EnemyData>();
        Skill = new List<SkillData>();
    }
}