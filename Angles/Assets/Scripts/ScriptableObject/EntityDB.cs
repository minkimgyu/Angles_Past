using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdditionalPrefabData
{
    [SerializeField]
    protected string name;
    public string Name { get { return name; } set { name = value; } }

    [SerializeField]
    protected int count;
    public int Count { get { return count; } set { count = value; } }

    [SerializeField]
    protected string path;
    public string Path { get { return path; } set { path = value; } }
}

[System.Serializable]
public class EntityData
{
    [SerializeField]
    protected string name;
    public string Name { get { return name; } set { name = value; } }

    [SerializeField]
    protected string shape;
    public string Shape { get { return shape; } set { shape = value; } }

    [SerializeField]
    protected string color;
    public string Color { get { return color; } set { color = value; } }

    [SerializeField]
    protected float hp;
    public float Hp { get { return hp; } set { hp = value; } }

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
    public EntityData(string name, string shape, string color, float hp, float speed, float stunTime, float weight, float drag, float knockBackThrust) 
    {
        this.name = name;
        this.shape = shape;
        this.color = color;
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

    public PlayerData(string name, string shape, string color, float hp, float speed, float stunTime, float weight, float drag, float knockBackThrust,  float readySpeed, float speedRatio, float minSpeedRatio, float maxSpeedRatio, float rushThrust,
        float rushRatio, float storedRushRatio, float rushTime, float attackCancelOffset, float reflectThrust, float maxDashCount, float dashTime, float dashThrust, float dashRatio) : base(name, shape, color, hp, speed, stunTime, weight, drag, knockBackThrust)
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
        return new PlayerData(name, shape, color, hp, speed, stunTime, weight, drag, knockBackThrust, readySpeed, speedRatio, minSpeedRatio, maxSpeedRatio, rushThrust,
        rushRatio, storedRushRatio, rushTime, attackCancelOffset, reflectThrust, maxDashCount, dashTime, dashThrust, dashRatio);
    }
}

[System.Serializable]
public class EnemyData : EntityData
{
    [SerializeField]
    protected float knockBackDamage;
    public float KnockBackDamage { get { return knockBackDamage; } set { knockBackDamage = value; } }

    [SerializeField]
    float skillMinDistance;
    public float SkillMinDistance { get { return skillMinDistance; } set { skillMinDistance = value; } }

    [SerializeField]
    float skillUseRange;
    public float SkillUseRange { get { return skillUseRange; } set { skillUseRange = value; } }

    [SerializeField]
    float followMinDistance;
    public float FollowMinDistance { get { return followMinDistance; } set { followMinDistance = value; } }

    [SerializeField]
    float stopMinDistance;
    public float StopMinDistance { get { return stopMinDistance; } set { stopMinDistance = value; } }

    [SerializeField]
    int prefabCount;
    public int PrefabCount { get { return prefabCount; } set { prefabCount = value; } }

    public EnemyData() { }
    public EnemyData(string name, string shape, string color, float hp, float speed, float stunTime, float weight, float drag, float knockBackThrust, float knockBackDamage, 
        float skillMinDistance, float skillUseRange, float followMinDistance, float stopMinDistance, int prefabCount) : base(name, shape, color, hp, speed, stunTime, weight, drag, knockBackThrust)
    {
        this.knockBackThrust = knockBackThrust;
        this.knockBackDamage = knockBackDamage;
        this.skillMinDistance = skillMinDistance;
        this.skillUseRange = skillUseRange;
        this.followMinDistance = followMinDistance;
        this.stopMinDistance = stopMinDistance;
        this.prefabCount = prefabCount;
    }

    public EnemyData CopyData()
    {
        return new EnemyData(name, shape, color, hp, speed, stunTime, weight, drag, knockBackThrust, knockBackDamage, skillMinDistance, skillUseRange, followMinDistance, stopMinDistance, prefabCount);
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
    SkillUsage usage;
    public SkillUsage Usage { get { return usage; } set { usage = value; } }

    [SerializeField]
    int useCount = 1;
    public int UseCount { get { return useCount; } set { useCount = value; } }

    [SerializeField]
    float useTick;
    public float UseTick { get { return useTick; } set { useTick = value; } }

    [SerializeField]
    float preDelay;
    public float PreDelay { get { return preDelay; } set { preDelay = value; } }

    [SerializeField]
    float duration;
    public float Duration { get { return duration; } set { duration = value; } }

    [SerializeField]
    float radiusRange;
    public float RadiusRange { get { return radiusRange; } set { radiusRange = value; } }

    [SerializeField]
    Vector2 boxRange;
    public Vector2 BoxRange { get { return boxRange; } set { boxRange = value; } }

    [SerializeField]
    Vector2 offsetRange;
    public Vector2 OffsetRange { get { return offsetRange; } set { offsetRange = value; } }

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

    [SerializeField]
    int prefabCount;
    public int PrefabCount { get { return prefabCount; } set { prefabCount = value; } }

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

    public void CountUp()
    {
        SkillData originData = DatabaseManager.Instance.ReturnSkillData(Name).CopyData();
        UseCount += originData.UseCount;
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

    public SkillData(SkillName name, SkillUseType useType, SkillUsage usage, int useCount, float useTick, float preDelay, float duration, float radiusRange, Vector2 boxRange, Vector2 offsetRange, float damage, float knockBackThrust, float disableTime, EntityTag[] hitTarget, int prefabCount)
    {
        this.name = name;
        this.useType = useType;
        this.usage = usage;
        this.useCount = useCount;
        this.useTick = useTick;
        this.preDelay = preDelay;
        this.duration = duration;
        this.radiusRange = radiusRange;
        this.boxRange = boxRange;
        this.offsetRange = offsetRange;
        this.damage = damage;
        this.knockBackThrust = knockBackThrust;
        this.disableTime = disableTime;
        this.hitTarget = hitTarget;
        this.prefabCount = prefabCount;
    }

    public SkillData CopyData()
    {
        SkillData skillData = new SkillData(name, useType, usage, useCount, useTick, preDelay, duration, radiusRange, boxRange, offsetRange, damage, knockBackThrust, disableTime, hitTarget, prefabCount);
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
    public List<AdditionalPrefabData> AdditionalPrefab;

    public void ResetData()
    {
        Player = new PlayerData();
        Enemy = new List<EnemyData>();
        Skill = new List<SkillData>();
        AdditionalPrefab = new List<AdditionalPrefabData>();
    }
}