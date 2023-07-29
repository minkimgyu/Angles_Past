using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class BuffFloat : IData<BuffFloat>
{
    [SerializeField]
    float maxValue;
    public float Max { get { return maxValue; } }

    [SerializeField]
    float minValue;
    public float Min { get { return minValue; } }

    [SerializeField]
    float originValue;
    public float OriginValue { get { return originValue; } set { originValue = value; } }
    public float IntervalValue
    {
        get
        {
            if(originValue < minValue)
            {
                return minValue;
            }
            else if(originValue > maxValue)
            {
                return maxValue;
            }
            else
            {
                return originValue;
            }
        }
        set 
        { 
            originValue = value;
        }
    }

    public bool IsOutInterval()
    {
        if (originValue < minValue)
        {
            return true;
        }
        else if (originValue > maxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public BuffFloat(float max, float min, float value)
    {
        this.maxValue = max;
        this.minValue = min;
        this.originValue = value;
    }

    public BuffFloat CopyData()
    {
        return new BuffFloat(maxValue, minValue, originValue);
    }
}

[System.Serializable]
public class GrantedUtilization
{
    [SerializeField]
    List<string> skillNames = new List<string>();

    [SerializeField]
    List<string> buffNames = new List<string>();
    public List<string> BuffNames { get { return buffNames; } }

    public void LootSkillFromDB(BattleComponent component)
    {
        for (int i = 0; i < skillNames.Count; i++)
        {
            //Debug.Log(DatabaseManager.Instance.UtilizationDB.ReturnSkillData(skillNames[i]));
            //Debug.Log(component);
            component.LootingSkill(DatabaseManager.Instance.UtilizationDB.ReturnSkillData(skillNames[i]));
        }
    }

    public List<BuffData> LootBuffFromDB()
    {
        List<BuffData> tmpData = new List<BuffData>();

        for (int i = 0; i < buffNames.Count; i++)
        {
            tmpData.Add(DatabaseManager.Instance.UtilizationDB.ReturnBuffData(buffNames[i]));
        }

        return tmpData;
    }
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
    protected BuffFloat speed;
    public BuffFloat Speed { get { return speed; } set { speed = value; } }

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

    [SerializeField]
    protected GrantedUtilization grantedUtilization;
    public GrantedUtilization GrantedUtilization { get { return grantedUtilization; } }

    public HealthEntityData() { }

    public HealthEntityData(string name, string shape, string color, bool immortality, float hp, BuffFloat speed, float stunTime, float weight, float drag, float knockBackThrust, GrantedUtilization grantedUtilization) : base(name)
    {
        this.shape = shape;
        this.color = color;
        this.immortality = immortality;
        this.hp = hp;

        this.speed = speed.CopyData();

        this.stunTime = stunTime;
        this.weight = weight;
        this.drag = drag;
        this.knockBackThrust = knockBackThrust;
        this.grantedUtilization = grantedUtilization;
    }
}

[System.Serializable]
public class PlayerData : HealthEntityData, IData<PlayerData>
{
    [SerializeField]
    BuffFloat readySpeed;
    public BuffFloat ReadySpeed { get { return readySpeed; } set { readySpeed = value; } }

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
            ratio += recoverRatio * Time.deltaTime * 100;
            if(ratio > 1) ratio = 1;

            return true;
        }
        else
        {
            return false;
        }
    }

    public PlayerData() { }

    public PlayerData(string name, string shape, string color, bool immortality, float hp, BuffFloat speed, float stunTime, float weight, float drag, float knockBackThrust, GrantedUtilization grantedUtilization, BuffFloat readySpeed, float speedRatio, float minSpeedRatio, float maxSpeedRatio, float rushThrust,
        float rushRatio, float rushRecoverRatio, float rushTime, float attackCancelOffset, float reflectThrust, float maxDashCount, float dashTime, float dashThrust, float dashRatio, float dashRecoverRatio) : base(name, shape, color, immortality, hp, speed, stunTime, weight, drag, knockBackThrust, grantedUtilization)
    {
        this.readySpeed = readySpeed.CopyData();
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
        return new PlayerData(name, shape, color, immortality, hp, speed, stunTime, weight, drag, knockBackThrust, grantedUtilization, readySpeed, speedRatio, minSpeedRatio, maxSpeedRatio, rushThrust,
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
    float skillUseDistance;
    public float SkillUseDistance { get { return skillUseDistance; } set { skillUseDistance = value; } }

    [SerializeField]
    float skillUseOffsetDistance;
    public float SkillUseOffsetDistance { get { return skillUseOffsetDistance; } set { skillUseOffsetDistance = value; } }

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

    public EnemyData(string name, string shape, string color, bool immortality, float hp, BuffFloat speed, float stunTime, float weight, float drag, float knockBackThrust, GrantedUtilization grantedUtilization, float knockBackDamage, 
        float skillUseDistance, float skillUseOffsetDistance, float skillUseRange, float skillReuseTime, float followMinDistance, float stopMinDistance, int prefabCount) : base(name, shape, color, immortality, hp, speed, stunTime, weight, drag, knockBackThrust, grantedUtilization)
    {
        this.knockBackDamage = knockBackDamage;
        this.skillUseDistance = skillUseDistance;
        this.skillUseOffsetDistance = skillUseOffsetDistance;
        this.skillUseRange = skillUseRange;
        this.skillReuseTime = skillReuseTime;
        this.followMinDistance = followMinDistance;
        this.stopMinDistance = stopMinDistance;
        this.prefabCount = prefabCount;
    }

    public EnemyData CopyData()
    {
        return new EnemyData(name, shape, color, immortality, hp, speed, stunTime, weight, drag, knockBackThrust, grantedUtilization, knockBackDamage, skillUseDistance, skillUseOffsetDistance, skillUseRange, skillReuseTime, followMinDistance, stopMinDistance, prefabCount);
    }
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

    public EnemyData ReturnEnemyData(string name)
    {
        return enemy.Find(x => x.Name == name).CopyData();
    }
    public void ResetData()
    {
        player = new PlayerData();
        enemy = new List<EnemyData>();
    }
}