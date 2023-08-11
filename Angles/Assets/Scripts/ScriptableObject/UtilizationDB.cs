using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData : IData<BuffData>
{
    [SerializeField]
    protected int maxCount;
    public int MaxCount { get { return maxCount; } set { maxCount = value; } }

    [SerializeField]
    float duration;
    public float Duration { get { return duration; } set { duration = value; } }

    [SerializeField]
    int tickCount;
    public int TickTime { get { return tickCount; } set { tickCount = value; } }

    public BuffData(int maxCount, float duration, int tickCount)
    {
        this.maxCount = maxCount;
        this.duration = duration;
        this.tickCount = tickCount;
    }

    public BuffData CopyData()
    {
        return new BuffData(maxCount, duration, tickCount);
    }
}

[System.Serializable]
public class SkillData : IData<SkillData>
{
    [Header("Sound")]
    [SerializeField]
    string sfxName;
    public string SfxName { get { return sfxName; } set { sfxName = value; } }

    [SerializeField]
    float volume;
    public float Volume { get { return volume; } set { volume = value; } }

    [Header("Call")]

    [SerializeField]
    string prefabName;
    public string PrefabName { get { return prefabName; } set { prefabName = value; } }

    [SerializeField]
    int maxUseCount;
    public int MaxUseCount { get { return maxUseCount; } set { maxUseCount = value; } }

    [SerializeField]
    int useCount;
    public int UseCount { get { return useCount; } set { useCount = value; } }

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
    List<float> radiusRangePerTick;
    public List<float> RadiusRangePerTick { get { return radiusRangePerTick; }}

    [SerializeField]
    float radiusRange;
    public float RadiusRange { get { return radiusRange; } set { radiusRange = value; } }

    [SerializeField]
    Vector2 boxRange;
    public Vector2 BoxRange { get { return boxRange; } set { boxRange = value; } }

    [SerializeField]
    Vector2 offsetRange;
    public Vector2 OffsetRange { get { return offsetRange; } set { offsetRange = value; } }


    [Header("Direction")]

    [SerializeField]
    public List<Vector3> directions;
    public List<Vector3> Directions { get { return directions; } set { directions = value; } }

    [Header("Spawn")]

    [SerializeField]
    int spawnCount;
    public int SpawnCount { get { return spawnCount; } set { spawnCount = value; } }

    [Header("Rotation")]
    [SerializeField]
    float rotationSpeed;
    public float RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }

    [Header("Disable")]

    [SerializeField]
    float disableTime;
    public float DisableTime { get { return disableTime; } set { disableTime = value; } }

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

    public SkillData(string sfxName, float volume, string prefabName, int maxUseCount, int useCount, SkillOverlapType overlapType, SkillUseConditionType useConditionType, SkillUseCountSubtractType countSubtractType, SkillSynthesisType synthesisType,
        int tickCount, float preDelay, float duration, float radiusRange, Vector2 boxRange, Vector2 offsetRange, float damage, float knockBackThrust, float disableTime, EntityTag[] hitTarget, int prefabCount, 
        int spawnCount, float rotationSpeed)
    {
        this.sfxName = sfxName;
        this.volume = volume;

        this.prefabName = prefabName;
        this.maxUseCount = maxUseCount;
        this.useCount = useCount; 
        this.overlapType = overlapType;
        this.useConditionType = useConditionType;
        this.countSubtractType = countSubtractType;
        this.synthesisType = synthesisType;

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

        this.spawnCount = spawnCount;

        this.rotationSpeed = rotationSpeed;
    }


    public SkillData CopyData()
    {
        return new SkillData(sfxName, volume, prefabName, maxUseCount, useCount, overlapType, useConditionType, countSubtractType, synthesisType, tickCount, preDelay, duration, radiusRange, boxRange, offsetRange, damage, knockBackThrust, 
            disableTime, hitTarget, prefabCount, spawnCount, rotationSpeed);
    }
}

[CreateAssetMenu(fileName = "UtilizationDB", menuName = "Scriptable Object/DB/UtilizationDB")]
public class UtilizationDB : ScriptableObject
{
    [Header("SkillData")]
    [SerializeField]
    Dictionary<string, SkillData> skillDatas;
    public Dictionary<string, SkillData> SkillDatas { get { return skillDatas; } }

    [SerializeField]
    Dictionary<string, DamageSupportData> damageSupportDatas;
    public Dictionary<string, DamageSupportData> DamageSupportDatas { get { return damageSupportDatas; } }

    [SerializeField]
    Dictionary<string, SpawnSupportData> spawnSupportDatas;
    public Dictionary<string, SpawnSupportData> SpawnSupportDatas { get { return spawnSupportDatas; } }

    [SerializeField]
    Dictionary<string, BuffSupportData> buffSupportDatas;
    public Dictionary<string, BuffSupportData> BuffSupportDatas { get { return buffSupportDatas; } }


    [Header("BuffData")]
    [SerializeField]
    Dictionary<string, BuffData> buffDatas;
    public Dictionary<string, BuffData> BuffDatas { get { return buffDatas; } }

    [SerializeField]
    Dictionary<string, HealthEntityData.BuffVariation> healthEntityDataVariation;
    public Dictionary<string, HealthEntityData.BuffVariation> HealthEntityDataVariation { get { return healthEntityDataVariation; } }

    [SerializeField]
    Dictionary<string, PlayerData.BuffVariation> playerDataVariation;
    public Dictionary<string, PlayerData.BuffVariation> PlayerDataVariation { get { return playerDataVariation; } }

    [SerializeField]
    Dictionary<string, BaseEnemyData.BuffVariation> baseEnemyDataVariation;
    public Dictionary<string, BaseEnemyData.BuffVariation> BaseEnemyDataVariation { get { return baseEnemyDataVariation; } }

    [SerializeField]
    Dictionary<string, FollowEnemyData.BuffVariation> followEnemyDataVariation;
    public Dictionary<string, FollowEnemyData.BuffVariation> FollowEnemyDataVariation { get { return followEnemyDataVariation; } }
}