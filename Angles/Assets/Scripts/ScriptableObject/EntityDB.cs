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

    [SerializeField]
    protected float weight;
    public float Weight { get { return weight; } set { weight = value; } }

    [SerializeField]
    protected float drag;
    public float Drag { get { return drag; } set { drag = value; } }

    [SerializeField]
    protected float knockBackThrust;
    public float KnockBackThrust { get { return knockBackThrust; } set { knockBackThrust = value; } }

    public EntityData() { }
    public EntityData(string name, float hp, float speed, float stunTime, float weight, float drag, float knockBackThrust) 
    {
        this.name = name;
        this.hp = hp;
        this.speed = speed;
        this.stunTime = stunTime;
        this.weight = weight;
        this.drag = drag;
        this.knockBackThrust = knockBackThrust;
    }
}

[System.Serializable]
public class PlayerData : EntityData
{
    [SerializeField]
    float readySpeed = 5;
    public float ReadySpeed { get { return readySpeed; } set { readySpeed = value; } }

    [SerializeField]
    float speedRatio;
    public float SpeedRatio { get { return speedRatio; } set { speedRatio = value; } }

    [SerializeField]
    float minSpeedRatio;
    public float MinSpeedRatio { get { return minSpeedRatio; } set { minSpeedRatio = value; } }

    [SerializeField]
    float maxSpeedRatio;
    public float MaxSpeedRatio { get { return maxSpeedRatio; } set { maxSpeedRatio = value; } }

    [SerializeField]
    float rushThrust;
    public float RushThrust { get { return rushThrust; } set { rushThrust = value; } }

    [SerializeField]
    float rushRatio;
    public float RushRatio { get { return rushRatio; } set { rushRatio = value; } }

    [SerializeField]
    float storedRushRatio;
    public float StoredRushRatio { get { return storedRushRatio; } set { storedRushRatio = value; } }

    [SerializeField]
    float rushTime;
    public float RushTime { get { return rushTime; } set { rushTime = value; } }

    [SerializeField]
    float attackCancelOffset;
    public float AttackCancelOffset { get { return attackCancelOffset; } set { attackCancelOffset = value; } }


    [SerializeField]
    float reflectThrust;
    public float ReflectThrust { get { return reflectThrust; } set { reflectThrust = value; } }


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

    public bool CanUseDash()
    {
        if (DashRatio - 1 / MaxDashCount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SubtractRatio()
    {
        DashRatio -= 1 / MaxDashCount;
    }

    public PlayerData() : base() { }

    public PlayerData(string name, float hp, float speed, float stunTime, float weight, float drag, float knockBackThrust,  float readySpeed, float speedRatio, float minSpeedRatio, float maxSpeedRatio, float rushThrust,
        float rushRatio, float storedRushRatio, float rushTime, float attackCancelOffset, float reflectThrust, float maxDashCount, float dashTime, float dashThrust, float dashRatio) : base(name, hp, speed, stunTime, weight, drag, knockBackThrust)
    {
        this.readySpeed = readySpeed;
        this.speedRatio = speedRatio;
        this.minSpeedRatio = minSpeedRatio;
        this.maxSpeedRatio = maxSpeedRatio;
        this.rushThrust = rushThrust;
        this.rushRatio = rushRatio;
        this.storedRushRatio = storedRushRatio;
        this.rushTime = rushTime;
        this.attackCancelOffset = attackCancelOffset;
        this.reflectThrust = reflectThrust;
        this.maxDashCount = maxDashCount;
        this.dashTime = dashTime;
        this.dashThrust = dashThrust;
        this.dashRatio = dashRatio;
    }

    public PlayerData CopyData()
    {
        return new PlayerData(name, hp, speed, stunTime, weight, drag, knockBackThrust, readySpeed, speedRatio, minSpeedRatio, maxSpeedRatio, rushThrust,
        rushRatio, storedRushRatio, rushTime, attackCancelOffset, reflectThrust, maxDashCount, dashTime, dashThrust, dashRatio);
    }
}

[System.Serializable]
public class EnemyData : EntityData
{
    [SerializeField]
    float skillUseRange;
    public float SkillUseRange { get { return skillUseRange; } set { skillUseRange = value; } }

    [SerializeField]
    float followMinDistance;
    public float FollowMinDistance { get { return followMinDistance; } set { followMinDistance = value; } }

    [SerializeField]
    float stopMinDistance;
    public float StopMinDistance { get { return stopMinDistance; } set { stopMinDistance = value; } }

    public EnemyData() { }
    public EnemyData(string name, float hp, float speed, float stunTime, float weight, float drag, float knockBackThrust, float skillUseRange, float followMinDistance, float stopMinDistance) : base(name, hp, speed, stunTime, weight, drag, knockBackThrust)
    {
        this.skillUseRange = skillUseRange;
        this.followMinDistance = followMinDistance;
        this.stopMinDistance = stopMinDistance;
    }

    public EnemyData CopyData()
    {
        return new EnemyData(name, hp, speed, stunTime, weight, drag, knockBackThrust, skillUseRange, followMinDistance, stopMinDistance);
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
    float knockBackThrust;
    public float KnockBackThrust { get { return knockBackThrust; } set { knockBackThrust = value; } }

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

    public bool CanHitSkill(string tag)
    {
        for (int i = 0; i < hitTarget.Length; i++)
        {
            if(hitTarget[i].ToString() == tag)
            {
                return true;
            }
        }

        return false;
    }

    public SkillData() { }

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