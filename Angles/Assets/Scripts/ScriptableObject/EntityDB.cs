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

public interface IBuffApplier<T>
{
    void ApplyBuff(T value);

    void RemoveBuff(T value);
}

/// <summary>
/// 버프에 사용되는 변수(Int)
/// </summary>
[System.Serializable]
public class BuffInt : IData<BuffFloat>
{
    [SerializeField]
    int maxValue;
    public int Max { get { return maxValue; } }

    [SerializeField]
    int minValue;
    public int Min { get { return minValue; } }

    [SerializeField]
    int originValue;
    public int OriginValue { get { return originValue; } set { originValue = value; } }
    public int IntervalValue
    {
        get
        {
            if (originValue < minValue)
            {
                return minValue;
            }
            else if (originValue > maxValue)
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

    public BuffInt(int max, int min, int value)
    {
        maxValue = max;
        minValue = min;
        originValue = value;
    }

    public BuffFloat CopyData()
    {
        return new BuffFloat(maxValue, minValue, originValue);
    }
}

/// <summary>
/// 버프에 사용되는 변수(Float)
/// </summary>
[System.Serializable]
public class BuffFloat : IData<BuffFloat>
{
    /// <summary>
    /// 최대값
    /// </summary>
    [SerializeField]
    float maxValue;
    public float Max { get { return maxValue; } }

    /// <summary>
    /// 최소값
    /// </summary>
    [SerializeField]
    float minValue;
    public float Min { get { return minValue; } }

    /// <summary>
    /// 실질적 값
    /// </summary>
    [SerializeField]
    float originValue;
    public float Origin { get { return originValue; } }

    /// <summary>
    /// 값 변경, 최대 - 최소 사이의 값을 리턴에 쓰이는 변수
    /// </summary>
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

    public BuffFloat(float max, float min, float value)
    {
        maxValue = max;
        minValue = min;
        originValue = value;
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

    public void LootSkillFromDB(BattleComponent component)
    {
        for (int i = 0; i < skillNames.Count; i++)
        {
            component.LootingSkill(DatabaseManager.Instance.UtilizationDB.SkillDatas[skillNames[i]].CopyData());
        }
    }
}

[System.Serializable] // 데이터를 상속으로 처리하지 말고 나눠보자
public class HealthEntityData : IData<HealthEntityData>, IBuffApplier<HealthEntityData.BuffVariation>
{
    [System.Serializable]
    public struct BuffVariation
    {
        public float hpVariation;
        public float speedVariation;
        public float stunTimeVariation;
        public float weightVariation;
        public float massVariation;
        public float dragVariation;
    }

    public void ApplyBuff(BuffVariation value)
    {
        Hp.IntervalValue += value.hpVariation;

        Speed.IntervalValue += value.speedVariation;

        StunTime.IntervalValue += value.stunTimeVariation;

        Weight.IntervalValue += value.weightVariation;
        Mass.IntervalValue += value.massVariation;
        Drag.IntervalValue += value.dragVariation;
    }

    public void RemoveBuff(BuffVariation value)
    {
        Hp.IntervalValue -= value.hpVariation;

        Speed.IntervalValue -= value.speedVariation;

        StunTime.IntervalValue -= value.stunTimeVariation;

        Weight.IntervalValue -= value.weightVariation;
        Mass.IntervalValue -= value.massVariation;
        Drag.IntervalValue -= value.dragVariation;
    }

    [SerializeField]
    protected bool immortality;
    public bool Immortality { get { return immortality; } set { immortality = value; } }

    [SerializeField]
    protected BuffFloat hp;
    public BuffFloat Hp { get { return hp; } set { hp = value.CopyData(); } }

    [SerializeField]
    protected BuffFloat speed;
    public BuffFloat Speed { get { return speed; } set { speed = value.CopyData(); } }

    [SerializeField]   
    protected BuffFloat stunTime;
    public BuffFloat StunTime { get { return stunTime; } set { stunTime = value.CopyData(); } }

    [SerializeField]
    protected BuffFloat weight;
    public BuffFloat Weight { get { return weight; } set { weight = value.CopyData(); } } // 실질적 Entity 무게

    [SerializeField]
    protected BuffFloat mass;
    public BuffFloat Mass { get { return mass; } set { mass = value.CopyData(); } } // rigidbody mass

    [SerializeField]
    protected BuffFloat drag;
    public BuffFloat Drag { get { return drag; } set { drag = value.CopyData(); } }

    [SerializeField]
    protected string dieEffectName;
    public string DieEffectName { get { return dieEffectName; }}

    public HealthEntityData() { }

    public HealthEntityData(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime, BuffFloat weight, BuffFloat mass, BuffFloat drag)
    {
        Immortality = immortality;
        Hp = hp;

        Speed = speed;

        StunTime = stunTime;

        Weight = mass;
        Mass = weight;
        Drag = drag;
    }

    public HealthEntityData CopyData()
    {
        return new HealthEntityData(immortality, hp, speed, stunTime, weight, mass, drag);
    }
}

[System.Serializable]
public class PlayerData : IData<PlayerData>, IBuffApplier<PlayerData.BuffVariation>
{
    [System.Serializable]
    public struct BuffVariation
    {
        public float readySpeedDecreaseRatioVariation;

        public float rushThrustVariation;
        public float rushRecoverRatioVariation;
        public float rushDurationVariation;

        public int dashCountVariation;
        public float dashDurationVariation;
        public float dashThrustVariation;
        public float dashRecoverRatioVariation;
    }

    public void ApplyBuff(BuffVariation value)
    {
        readySpeedDecreaseRatio.IntervalValue += value.readySpeedDecreaseRatioVariation;

        rushThrust.IntervalValue += value.rushThrustVariation;
        rushRecoverRatio.IntervalValue += value.rushRecoverRatioVariation;
        rushDuration.IntervalValue += value.rushDurationVariation;


        dashCount.IntervalValue = value.dashCountVariation;
        dashDuration.IntervalValue = value.dashDurationVariation;
        dashThrust.IntervalValue = value.dashThrustVariation;
        dashRecoverRatio.IntervalValue = value.dashRecoverRatioVariation;
    }

    public void RemoveBuff(BuffVariation value)
    {
        readySpeedDecreaseRatio.IntervalValue -= value.readySpeedDecreaseRatioVariation;

        rushThrust.IntervalValue -= value.rushThrustVariation;
        rushRecoverRatio.IntervalValue -= value.rushRecoverRatioVariation;
        rushDuration.IntervalValue -= value.rushDurationVariation;


        dashCount.IntervalValue -= value.dashCountVariation;
        dashDuration.IntervalValue -= value.dashDurationVariation;
        dashThrust.IntervalValue -= value.dashThrustVariation;
        dashRecoverRatio.IntervalValue -= value.dashRecoverRatioVariation;
    }

    [SerializeField]
    BuffFloat readySpeedDecreaseRatio;
    public BuffFloat ReadySpeedDecreaseRatio { get { return readySpeedDecreaseRatio; } set { readySpeedDecreaseRatio = value; } }

    [SerializeField]
    BuffFloat rushThrust;
    public BuffFloat RushThrust { get { return rushThrust; } set { rushThrust = value; } }

    [SerializeField]
    float rushRatio; // 범위 0 ~ 1까지
    public float RushRatio { get { return rushRatio; } set { rushRatio = value; } }

    [SerializeField]
    BuffFloat rushRecoverRatio; // 회복 비율
    public BuffFloat RushRecoverRatio { get { return rushRecoverRatio; } set { rushRecoverRatio = value; } }

    [SerializeField]
    BuffFloat rushDuration;
    public BuffFloat RushDuration { get { return rushDuration; } set { rushDuration = value; } }

    /// <summary>
    /// 조이스틱 움직임 값에 따라 AttackReady에서 Move 상태로 왔다갔다함
    /// </summary>
    [SerializeField]
    float attackCancelOffset;
    public float AttackCancelOffset { get { return attackCancelOffset; } set { attackCancelOffset = value; } }


    [SerializeField]
    BuffInt dashCount;
    public BuffInt DashCount { get { return dashCount; } set { dashCount = value; } }


    [SerializeField]
    BuffFloat dashDuration;
    public BuffFloat DashDuration { get { return dashDuration; } set { dashDuration = value; } }


    [SerializeField]
    BuffFloat dashThrust;
    public BuffFloat DashThrust { get { return dashThrust; } set { dashThrust = value; } }

    /// <summary>
    /// 대쉬 비율
    /// </summary>
    [SerializeField]
    float dashRatio;
    public float DashRatio { get { return dashRatio; } set { dashRatio = value; } }


    /// <summary>
    /// 대쉬 회복량
    /// </summary>
    [SerializeField]
    BuffFloat dashRecoverRatio;
    public BuffFloat DashRecoverRatio { get { return dashRecoverRatio; } set { dashRecoverRatio = value; } }

    [SerializeField]
    protected GrantedUtilization grantedUtilization;
    public GrantedUtilization GrantedUtilization { get { return grantedUtilization; } }

    public bool CanUseDash()
    {
        if (DashRatio - 1 / DashCount.OriginValue >= 0)
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
        DashRatio -= 1 / DashCount.OriginValue;
    }

    public bool RestoreDashRatio() { return RestoreRatio(ref dashRatio, dashRecoverRatio.IntervalValue); }

    public bool RestoreRushRatio() { return RestoreRatio(ref rushRatio, rushRecoverRatio.IntervalValue); }

    public void ResetRushRatioToZero() => rushRatio = 0;

    public bool RestoreRatio(ref float ratio, float recoverRatio)
    {
        if(ratio < 1)
        {
            ratio += recoverRatio * Time.deltaTime;
            if(ratio > 1) ratio = 1;

            return true;
        }
        else
        {
            return false;
        }
    }

    public PlayerData() { }

    public PlayerData(BuffFloat readySpeedDecreaseRatio, BuffFloat rushThrust, float rushRatio, BuffFloat rushRecoverRatio, BuffFloat rushDuration, float attackCancelOffset, 
        BuffInt dashCount, BuffFloat dashDuration, BuffFloat dashThrust, float dashRatio, BuffFloat dashRecoverRatio, GrantedUtilization grantedUtilization)
    {
        this.readySpeedDecreaseRatio = readySpeedDecreaseRatio;
        this.rushThrust = rushThrust;
        this.rushRatio = rushRatio;
        this.rushRecoverRatio = rushRecoverRatio;
        this.rushDuration = rushDuration;

        this.attackCancelOffset = attackCancelOffset;

        this.dashCount = dashCount;
        this.dashDuration = dashDuration;
        this.dashThrust = dashThrust;
        this.dashRatio = dashRatio;
        this.dashRecoverRatio = dashRecoverRatio;

        this.grantedUtilization = grantedUtilization;
    }

    public PlayerData CopyData()
    {
        return new PlayerData(readySpeedDecreaseRatio, rushThrust,
        rushRatio, rushRecoverRatio, rushDuration, attackCancelOffset, dashCount, dashDuration, dashThrust, dashRatio, dashRecoverRatio, grantedUtilization);
    }
}

[System.Serializable]
public class BaseEnemyData : IData<BaseEnemyData>, IBuffApplier<BaseEnemyData.BuffVariation> // 적끼리 겹치는 데이터 모음
{
    [System.Serializable]
    public struct BuffVariation
    {
        public int scoreVariation;
    }

    public void ApplyBuff(BuffVariation value)
    {
        score.IntervalValue += value.scoreVariation;
    }

    public void RemoveBuff(BuffVariation value)
    {
        score.IntervalValue -= value.scoreVariation;
    }

    [SerializeField]
    protected BuffInt score;
    public BuffInt Score { get { return score; } set { score = value; } }

    [SerializeField]
    protected GrantedUtilization grantedUtilization;
    public GrantedUtilization GrantedUtilization { get { return grantedUtilization; } }

    public BaseEnemyData(BuffInt score, GrantedUtilization grantedUtilization)
    {
        this.score = score;
        this.grantedUtilization = grantedUtilization;
    }

    public BaseEnemyData CopyData()
    {
        return new BaseEnemyData(score, grantedUtilization);
    }
}

[System.Serializable]
public class FollowEnemyData : IData<FollowEnemyData>, IBuffApplier<FollowEnemyData.BuffVariation> // 추적 하는 적 데이터 모음
{
    [System.Serializable]
    public struct BuffVariation
    {
        public float skillUseDistanceVariation;
        public float skillUseOffsetDistanceVariation;
        public float skillCooldownTimeVariation;
        public float followDistanceVariation;
        public float followOffsetDistanceVariation;
    }

    public void ApplyBuff(BuffVariation value)
    {
        skillUseDistance.IntervalValue += value.skillUseDistanceVariation;
        skillUseOffsetDistance.IntervalValue += value.skillUseOffsetDistanceVariation;
        skillCooldownTime.IntervalValue += value.skillCooldownTimeVariation;

        followDistance.IntervalValue += value.followDistanceVariation;
        followOffsetDistance.IntervalValue += value.followOffsetDistanceVariation;
    }

    public void RemoveBuff(BuffVariation value)
    {
        skillUseDistance.IntervalValue -= value.skillUseDistanceVariation;
        skillUseOffsetDistance.IntervalValue -= value.skillUseOffsetDistanceVariation;
        skillCooldownTime.IntervalValue -= value.skillCooldownTimeVariation;

        followDistance.IntervalValue -= value.followDistanceVariation;
        followOffsetDistance.IntervalValue -= value.followOffsetDistanceVariation;
    }

    [SerializeField]
    BuffFloat skillUseDistance;
    public BuffFloat SkillUseDistance { get { return skillUseDistance; } set { skillUseDistance = value; } }

    [SerializeField]
    BuffFloat skillUseOffsetDistance;
    public BuffFloat SkillUseOffsetDistance { get { return skillUseOffsetDistance; } set { skillUseOffsetDistance = value; } }

    [SerializeField]
    BuffFloat skillCooldownTime;
    public BuffFloat SkillReuseTime { get { return skillCooldownTime; } set { skillCooldownTime = value; } }

    [SerializeField]
    BuffFloat followDistance;
    public BuffFloat FollowDistance { get { return followDistance; } set { followDistance = value; } }

    [SerializeField]
    BuffFloat followOffsetDistance;
    public BuffFloat FollowOffsetDistance { get { return followOffsetDistance; } set { followOffsetDistance = value; } }

    public FollowEnemyData() { }

    public FollowEnemyData(BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance, BuffFloat skillCooldownTime, BuffFloat followDistance, BuffFloat followOffsetDistance)
    {
        this.skillUseDistance = skillUseDistance;
        this.skillUseOffsetDistance = skillUseOffsetDistance;
        this.skillCooldownTime = skillCooldownTime;
        this.followDistance = followDistance;
        this.followOffsetDistance = followOffsetDistance;
    }

    public FollowEnemyData CopyData()
    {
        return new FollowEnemyData(skillUseDistance, skillUseOffsetDistance, skillCooldownTime, followDistance, followOffsetDistance);
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