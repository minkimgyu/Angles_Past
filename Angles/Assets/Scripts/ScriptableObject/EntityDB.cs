using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class AdditionalPrefabData
//{
//    [SerializeField]
//    protected string name;
//    public string Name { get { return name; } set { name = value; } }

//    [SerializeField]
//    protected int count;
//    public int Count { get { return count; } set { count = value; } }

//    [SerializeField]
//    protected string path;
//    public string Path { get { return path; } set { path = value; } }
//}

public class BaseData
{
    [SerializeField]
    protected string name;
    public string Name { get { return name; } set { name = value; } }

    public BaseData() { }

    public BaseData(string name)
    {
        this.name = name;
    }
}

public interface IData<T>
{
    T CopyData();
}

[System.Serializable]
public class HealthEntityData : BaseData
{
    [SerializeField]
    protected string shape;
    public string Shape { get { return shape; } set { shape = value; } }

    [SerializeField]
    protected string color;
    public string Color { get { return color; } set { color = value; } }

    [SerializeField]
    protected bool immortality;
    public bool Immortality { get { return immortality; } set { immortality = value; } }

    [SerializeField]
    protected float hp;
    public float Hp { get { return hp; } set { hp = value; } }

    [SerializeField]
    protected float maxSpeed;
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    [SerializeField]
    protected float minSpeed;
    public float MinSpeed { get { return minSpeed; } set { minSpeed = value; } }

    [SerializeField]
    protected float speed;
    public float Speed {get { return speed; } set{ speed = value; } }
    

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

    public HealthEntityData() { }

    public HealthEntityData(string name, string shape, string color, bool immortality, float hp, float maxSpeed, float minSpeed, float speed, float stunTime, float weight, float drag, float knockBackThrust) : base(name)
    {
        this.shape = shape;
        this.color = color;
        this.immortality = immortality;
        this.hp = hp;
        this.maxSpeed = maxSpeed;
        this.minSpeed = minSpeed;
        this.speed = speed;
        this.stunTime = stunTime;
        this.weight = weight;
        this.drag = drag;
        this.knockBackThrust = knockBackThrust;
    }
}

[System.Serializable]
public class PlayerData : HealthEntityData, IData<PlayerData>
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
    float rushRecoverRatio;
    public float RushRecoverRatio { get { return rushRecoverRatio; } set { rushRecoverRatio = value; } }

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

    [SerializeField]
    float dashRecoverRatio;
    public float DashRecoverRatio { get { return dashRecoverRatio; } set { dashRecoverRatio = value; } }

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

    public void SubtractDashRatio()
    {
        DashRatio -= 1 / MaxDashCount;
    }

    public bool RestoreDashRatio() { return RestoreRatio(ref dashRatio, dashRecoverRatio); }

    public bool RestoreRushRatio() { return RestoreRatio(ref rushRatio, rushRecoverRatio); }

    public void ResetRushRatioToZero() => rushRatio = 0;

    public bool RestoreRatio(ref float ratio, float recoverRatio)
    {
        if(ratio < 1)
        {
            ratio += recoverRatio;
            if(ratio > 1) ratio = 1;

            return true;
        }
        else
        {
            return false;
        }
    }

    public PlayerData() { }

    public PlayerData(string name, string shape, string color, bool immortality, float hp, float maxSpeed, float minSpeed, float speed, float stunTime, float weight, float drag, float knockBackThrust,  float readySpeed, float speedRatio, float minSpeedRatio, float maxSpeedRatio, float rushThrust,
        float rushRatio, float rushRecoverRatio, float rushTime, float attackCancelOffset, float reflectThrust, float maxDashCount, float dashTime, float dashThrust, float dashRatio, float dashRecoverRatio) : base(name, shape, color, immortality, hp, maxSpeed, minSpeed, speed, stunTime, weight, drag, knockBackThrust)
    {
        this.readySpeed = readySpeed;
        this.speedRatio = speedRatio;
        this.minSpeedRatio = minSpeedRatio;
        this.maxSpeedRatio = maxSpeedRatio;
        this.rushThrust = rushThrust;
        this.rushRatio = rushRatio;
        this.rushRecoverRatio = rushRecoverRatio;
        this.rushTime = rushTime;
        this.attackCancelOffset = attackCancelOffset;
        this.reflectThrust = reflectThrust;
        this.maxDashCount = maxDashCount;
        this.dashTime = dashTime;
        this.dashThrust = dashThrust;
        this.dashRatio = dashRatio;
        this.dashRecoverRatio = dashRecoverRatio;
    }

    public PlayerData CopyData()
    {
        return new PlayerData(name, shape, color, immortality, hp, maxSpeed, minSpeed, speed, stunTime, weight, drag, knockBackThrust, readySpeed, speedRatio, minSpeedRatio, maxSpeedRatio, rushThrust,
        rushRatio, rushRecoverRatio, rushTime, attackCancelOffset, reflectThrust, maxDashCount, dashTime, dashThrust, dashRatio, dashRecoverRatio);
    }
}

[System.Serializable]
public class EnemyData : HealthEntityData, IData<EnemyData>
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
    float skillReuseTime;
    public float SkillReuseTime { get { return skillReuseTime; } set { skillReuseTime = value; } }

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

    public EnemyData(string name, string shape, string color, bool immortality, float hp, float maxSpeed, float minSpeed, float speed, float stunTime, float weight, float drag, float knockBackThrust, float knockBackDamage, 
        float skillMinDistance, float skillUseRange, float skillReuseTime, float followMinDistance, float stopMinDistance, int prefabCount) : base(name, shape, color, immortality, hp, maxSpeed, minSpeed, speed, stunTime, weight, drag, knockBackThrust)
    {
        this.knockBackThrust = knockBackThrust;
        this.knockBackDamage = knockBackDamage;
        this.skillMinDistance = skillMinDistance;
        this.skillUseRange = skillUseRange;
        this.skillReuseTime = skillReuseTime;
        this.followMinDistance = followMinDistance;
        this.stopMinDistance = stopMinDistance;
        this.prefabCount = prefabCount;
    }

    public EnemyData CopyData()
    {
        return new EnemyData(name, shape, color, immortality, hp, maxSpeed, minSpeed, speed, stunTime, weight, drag, knockBackThrust, knockBackDamage, skillMinDistance, skillUseRange, skillReuseTime, followMinDistance, stopMinDistance, prefabCount);
    }
}

[System.Serializable]
public class SkillCallData : BaseData, IData<SkillCallData>
{
    [SerializeField]
    int maxUseCount;
    public int MaxUseCount { get { return maxUseCount; } set { maxUseCount = value; } }

    [SerializeField]
    int useCount;
    public int UseCount { get { return useCount; } set { useCount = value; } }

    //[SerializeField]
    //Test test;
    //public Test Test1 { get { return test; } set { test = value; } }

    [SerializeField]
    SkillOverlapType overlapType;
    public SkillOverlapType OverlapType { get { return overlapType; } set { overlapType = value; } }

    [SerializeField]
    SkillUseConditionType useConditionType;
    public SkillUseConditionType UseConditionType { get { return useConditionType; } set { useConditionType = value; } }

    [SerializeField]
    SkillUseCountSubtractType countSubtractType;
    public SkillUseCountSubtractType CountSubtractType { get { return countSubtractType; } set { countSubtractType = value; } }

    [SerializeField]
    SkillSynthesisType synthesisType;
    public SkillSynthesisType SynthesisType { get { return synthesisType; } set { synthesisType = value; } }


    public bool CanUseSkill(SkillUseConditionType useConditionType)
    {
        return this.useConditionType == useConditionType;
    }

    public bool CanSubtractUseCount()
    {
        if (countSubtractType == SkillUseCountSubtractType.None) return false;
        useCount--;

        return true;
    }

    public bool IsUseCountZero()
    {
        return useCount <= 0;
    }

    public void UpUseCount()
    {
        if (synthesisType == SkillSynthesisType.None) return;

        useCount += maxUseCount;
    }

    public SkillCallData(string name, int maxUseCount, int useCount, SkillOverlapType overlapType, SkillUseConditionType useConditionType, SkillUseCountSubtractType countSubtractType, SkillSynthesisType synthesisType)
        :base(name)
    {
        this.maxUseCount = maxUseCount;
        this.useCount = useCount;
        this.overlapType = overlapType;
        this.useConditionType = useConditionType;
        this.countSubtractType = countSubtractType;
        this.synthesisType = synthesisType;
    }

    public SkillCallData CopyData()
    {
        return new SkillCallData(name, maxUseCount, useCount, overlapType, useConditionType, countSubtractType, synthesisType);
    }
}

[System.Serializable]
public class SkillData : BaseData, IData<SkillData>
{
    [SerializeField]
    float disableTime;
    public float DisableTime { get { return disableTime; } set { disableTime = value; } }


    [Header("Attack")]

    [SerializeField]
    float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    [SerializeField]
    float knockBackThrust;
    public float KnockBackThrust { get { return knockBackThrust; } set { knockBackThrust = value; } }

    [SerializeField]
    EntityTag[] hitTarget;
    public EntityTag[] HitTarget { get { return hitTarget; } set { hitTarget = value; } }


    [Header("PrefabCount")]

    [SerializeField]
    int prefabCount;
    public int PrefabCount { get { return prefabCount; } set { prefabCount = value; } }


    [Header("Delay")]

    [SerializeField]
    float preDelay;
    public float PreDelay { get { return preDelay; } set { preDelay = value; } }


    [Header("Tick")]

    [SerializeField]
    int tickCount = 1;
    public int TickCount { get { return tickCount; } set { tickCount = value; } }

    [SerializeField]
    float duration;
    public float Duration { get { return duration; } set { duration = value; } }


    [Header("Range")]

    [SerializeField]
    float radiusRange;
    public float RadiusRange { get { return radiusRange; } set { radiusRange = value; } }

    [SerializeField]
    Vector2 boxRange;
    public Vector2 BoxRange { get { return boxRange; } set { boxRange = value; } }

    [SerializeField]
    Vector2 offsetRange;
    public Vector2 OffsetRange { get { return offsetRange; } set { offsetRange = value; } }

    [Header("Spawn")]

    [SerializeField]
    int spawnCount;
    public int SpawnCount { get { return spawnCount; } set { spawnCount = value; } }

    [SerializeField]
    int spawnObjectSpeed;
    public int SpawnObjectSpeed { get { return spawnObjectSpeed; } set { spawnObjectSpeed = value; } }



    #region Fn

    public int ReturnLayerMask()
    {
        string[] hitTargetString = new string[hitTarget.Length];

        for (int i = 0; i < hitTarget.Length; i++)
        {
            hitTargetString[i] = hitTarget[i].ToString();
            Debug.Log(hitTargetString[i]);
        }

        Debug.Log(hitTargetString);
        return LayerMask.GetMask(hitTargetString);
    }

    //public bool CanUseSkill(SkillUseType skillType)
    //{
    //    return useCount >= 1;
    //}

    //public void AfterSkillAdjustment(List<SkillData> skillDatas)
    //{
    //    if (CanUseSkill(useType) == false) return;

    //    if (Usage == SkillUsage.Single) useCount -= 1;

    //    if (useCount <= 0)
    //    {
    //        skillDatas.Remove(this);
    //    }
    //}

    //public void CountCheckBySynthesis(SkillSynthesis synthesis)
    //{
    //    if (synthesis == SkillSynthesis.CountUp) CountUp();
    //    else if (synthesis == SkillSynthesis.Overlap) return;
    //}

    //public void CountUp()
    //{
    //    SkillData originData = DatabaseManager.Instance.ReturnSkillData(Name).CopyData();
    //    useCount += originData.useCount;
    //}

    public bool CanHitSkill(EntityTag tag)
    {
        for (int i = 0; i < hitTarget.Length; i++)
        {
            if (hitTarget[i] == tag)
            {
                return true;
            }
        }

        return false;
    }

    //public SkillData() { }

    public SkillData(string name, int tickCount, float preDelay, float duration, float radiusRange, Vector2 boxRange, Vector2 offsetRange, float damage, float knockBackThrust, float disableTime, EntityTag[] hitTarget, int prefabCount)
    : base(name)
    {
        this.tickCount = tickCount;
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
        return new SkillData(name, tickCount, preDelay, duration, radiusRange, boxRange, offsetRange, damage, knockBackThrust, disableTime, hitTarget, prefabCount);
    }

    #endregion
}

[CreateAssetMenu(fileName = "EntityDB", menuName = "Scriptable Object/DB/EntityDB")]
public class EntityDB : ScriptableObject
{
    [SerializeField]
    PlayerData player;
    public PlayerData Player { get { return player; } }

    [SerializeField]
    List<EnemyData> enemy;
    public List<EnemyData> Enemy { get { return enemy; } }

    public void ResetData()
    {
        player = new PlayerData();
        enemy = new List<EnemyData>();
    }
}