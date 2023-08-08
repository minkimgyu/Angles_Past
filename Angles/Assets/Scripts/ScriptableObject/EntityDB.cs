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

[System.Serializable] // 데이터를 상속으로 처리하지 말고 나눠보자
public class HealthEntityData : IData<HealthEntityData>
{
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
    protected string dieEffectName;
    public string DieEffectName { get { return dieEffectName; }}



    public HealthEntityData() { }

    public HealthEntityData(bool immortality, float hp, BuffFloat speed, float stunTime, float weight, float drag)
    {
        this.immortality = immortality;
        this.hp = hp;

        this.speed = speed.CopyData();

        this.stunTime = stunTime;
        this.weight = weight;
        this.drag = drag;
    }

    public HealthEntityData CopyData()
    {
        return new HealthEntityData(immortality, hp, speed, stunTime, weight, drag);
    }
}

[System.Serializable]
public class PlayerData : IData<PlayerData>
{
    [SerializeField]
    float readySpeedDecreaseRatio;
    public float ReadySpeedDecreaseRatio { get { return readySpeedDecreaseRatio; } set { readySpeedDecreaseRatio = value; } }

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
    float rushDuration;
    public float RushDuration { get { return rushDuration; } set { rushDuration = value; } }

    [SerializeField]
    float attackCancelOffset;
    public float AttackCancelOffset { get { return attackCancelOffset; } set { attackCancelOffset = value; } }


    [SerializeField]
    float maxDashCount;
    public float MaxDashCount { get { return maxDashCount; } set { maxDashCount = value; } }


    [SerializeField]
    float dashDuration;
    public float DashDuration { get { return dashDuration; } set { dashDuration = value; } }


    [SerializeField]
    float dashThrust;
    public float DashThrust { get { return dashThrust; } set { dashThrust = value; } }


    [SerializeField]
    float dashRatio;
    public float DashRatio { get { return dashRatio; } set { dashRatio = value; } }

    [SerializeField]
    float dashRecoverRatio;
    public float DashRecoverRatio { get { return dashRecoverRatio; } set { dashRecoverRatio = value; } }

    [SerializeField]
    protected GrantedUtilization grantedUtilization;
    public GrantedUtilization GrantedUtilization { get { return grantedUtilization; } }

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

    public PlayerData(float readySpeedDecreaseRatio, float rushThrust,
        float rushRatio, float rushRecoverRatio, float rushTime, float attackCancelOffset, float maxDashCount, float dashTime, float dashThrust, float dashRatio, float dashRecoverRatio, GrantedUtilization grantedUtilization)
    {
        this.readySpeedDecreaseRatio = readySpeedDecreaseRatio;
        this.rushThrust = rushThrust;
        this.rushRatio = rushRatio;
        this.rushRecoverRatio = rushRecoverRatio;
        this.rushDuration = rushTime; 
        this.maxDashCount = maxDashCount;
        this.dashDuration = dashTime;
        this.dashThrust = dashThrust;
        this.dashRatio = dashRatio;
        this.dashRecoverRatio = dashRecoverRatio;

        this.grantedUtilization = grantedUtilization;
    }

    public PlayerData CopyData()
    {
        return new PlayerData(readySpeedDecreaseRatio, rushThrust,
        rushRatio, rushRecoverRatio, rushDuration, attackCancelOffset, maxDashCount, dashDuration, dashThrust, dashRatio, dashRecoverRatio, grantedUtilization);
    }
}

[System.Serializable]
public class BaseEnemyData : IData<BaseEnemyData> // 적끼리 겹치는 데이터 모음
{
    [SerializeField]
    protected int score;
    public int Score { get { return score; } set { score = value; } }

    [SerializeField]
    protected float spawnPercentage;
    public float SpawnPercentage { get { return spawnPercentage; } set { spawnPercentage = value; } }

    [SerializeField]
    protected GrantedUtilization grantedUtilization;
    public GrantedUtilization GrantedUtilization { get { return grantedUtilization; } }

    public BaseEnemyData(int score, float spawnPercentage, GrantedUtilization grantedUtilization)
    {
        this.score = score;
        this.spawnPercentage = spawnPercentage;
        this.grantedUtilization = grantedUtilization;
    }

    public BaseEnemyData CopyData()
    {
        return new BaseEnemyData(score, spawnPercentage, grantedUtilization);
    }
}

[System.Serializable]
public class FollowEnemyData : IData<FollowEnemyData> // 추적 하는 적 데이터 모음
{
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
    float skillCooldownTime;
    public float SkillReuseTime { get { return skillCooldownTime; } set { skillCooldownTime = value; } }

    [SerializeField]
    float followMinDistance;
    public float FollowMinDistance { get { return followMinDistance; } set { followMinDistance = value; } }

    [SerializeField]
    float stopMinDistance;
    public float StopMinDistance { get { return stopMinDistance; } set { stopMinDistance = value; } }

    public FollowEnemyData() { }

    public FollowEnemyData(float skillUseDistance, float skillUseOffsetDistance, float skillUseRange, float skillCooldownTime, float followMinDistance, float stopMinDistance)
    {
        this.skillUseDistance = skillUseDistance;
        this.skillUseOffsetDistance = skillUseOffsetDistance;
        this.skillUseRange = skillUseRange;
        this.skillCooldownTime = skillCooldownTime;
        this.followMinDistance = followMinDistance;
        this.stopMinDistance = stopMinDistance;
    }

    public FollowEnemyData CopyData()
    {
        return new FollowEnemyData(skillUseDistance, skillUseOffsetDistance, skillUseRange, skillCooldownTime, followMinDistance, stopMinDistance);
    }
}

public class PlayerStat
{
    [SerializeField]
    HealthEntityData healthData;
    public HealthEntityData HealthData { get { return healthData; } }

    [SerializeField]
    PlayerData playerData;
    public PlayerData PlayerData { get { return playerData; } }

    
}

public class FollowEnemyStat
{
    [SerializeField]
    HealthEntityData healthData;
    public HealthEntityData HealthData { get { return healthData; } }

    [SerializeField]
    BaseEnemyData baseEnemyData;
    public BaseEnemyData BaseEnemyData { get { return baseEnemyData; } }

    [SerializeField]
    FollowEnemyData followEnemyData;
    public FollowEnemyData FollowEnemyData { get { return followEnemyData; } }
}

public class ReflectEnemyStat
{
    [SerializeField]
    HealthEntityData healthData;
    public HealthEntityData HealthData { get { return healthData; } }

    [SerializeField]
    BaseEnemyData baseEnemyData;
    public BaseEnemyData BaseEnemyData { get { return baseEnemyData; } }
}

[CreateAssetMenu(fileName = "EntityDB", menuName = "Scriptable Object/DB/EntityDB")]
public class EntityDB : ScriptableObject
{
    [SerializeField]
    PlayerStat playerStat;
    public PlayerStat PlayerStat { get { return playerStat; } }

    [SerializeField]
    StringFollowEnemyStatDictionary followEnemyStats;
    public StringFollowEnemyStatDictionary FollowEnemyStats { get { return followEnemyStats; } }

    [SerializeField]
    StringReflectEnemyStatDictionary reflectEnemyStats;
    public StringReflectEnemyStatDictionary ReflectEnemyStats { get { return reflectEnemyStats; } }
}