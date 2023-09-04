using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public struct SkillSupportData // --> ���Ŀ� ���� �߰�
{
    public SkillSupportData(GameObject caster, Vector3 pos, int tickCount)//, int tickCount)
    {
        m_caster = caster;
        m_Pos = pos;
        m_tickCount = tickCount;
    }

    /// <summary>
    /// ��ų ������
    /// </summary>
    GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }



    /// <summary>
    /// ��ų�� ������ ��ġ
    /// </summary>
    Vector3 m_Pos;
    public Vector3 Pos { get { return m_Pos; } }

    /// <summary>
    /// ���� TickCount
    /// </summary>
    int m_tickCount;
    public int TickCount { get { return m_tickCount; } }
}

public enum EffectCondition // ���� ��, ǥ�鿡 ����� ����Ʈ, ��ų ��ü ����Ʈ ��� Ư�� ���ǿ� ����� �ܾ �־���´�.
{
    PredelayEffect, // ��ų ���� ���� ����Ʈ

    HitSurfaceEffect, // �������� ���� ǥ�鿡 �Ͼ�� ȿ��

    AttackEffect, // ���� ��ų�� ������� �� ������ ȿ��

    BuffEffect, // ������ �ɸ� �� ������ ȿ��

    SpawnEffect // ������ �Ǿ��� �� ������ ȿ��
}




//public struct AudioVisualData
//{
//    public string effectName;
//    public float disableTime;

//    public string soundName;
//    public float volume;

//    public AudioVisualData(string effectName, float disableTime = -1, string soundName = null, float volume = 0)
//    {
//        this.effectName = effectName;
//        this.disableTime = disableTime;
//        this.soundName = soundName;
//        this.volume = volume;
//    }
//}

//// EffectSource
//// ��ǥ: Effect�� Sound�� ���� ������ش�.
//public class AudioVisualPlayer
//{
//    BasicEffectPlayer effectPlayer;
//    SoundPlayer soundPlayer;
//    AudioVisualData data;

//    public EffectSource(AudioVisualData effectData) // ���� �÷��� �̹�Ʈ Ʈ����, ����Ʈ ������
//    {
//        this.effectData = effectData;
//    }

//    public bool Play(Transform target)
//    {
//        BasicEffectPlayer playerTransform = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // ����Ʈ �̸� �߰�
//        if (playerTransform == null) return false;

//        playerTransform.IsFixed = true;
//        playerTransform.AddState(target, effectData.disableTime);
//        playerTransform.PlayEffect();


//        if(effectData.soundName != null)
//            SoundManager.instance.PlaySFX(target.position, effectData.soundName, effectData.volume);

//        effectPlayer = playerTransform;
//        return true;
//    }

//    public bool Play(Vector3 pos)
//    {
//        BasicEffectPlayer playerTransform = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // ����Ʈ �̸� �߰�
//        if (playerTransform == null) return false;

//        playerTransform.IsFixed = false;
//        playerTransform.AddState(pos, effectData.disableTime);
//        playerTransform.PlayEffect();

//        if (effectData.soundName != null)
//            SoundManager.instance.PlaySFX(pos, effectData.soundName, effectData.volume);

//        effectPlayer = playerTransform;
//        return true;
//    }

//    public void Stop()
//    {
//        effectPlayer.StopEffect();
//        effectPlayer = null;
//    }
//}

public class EffectSpawnComponent // --> �̰Ÿ� BaseMethod�� ��ӽ��Ѽ� ���� ���� + �߰��� ��ų Ŭ�������� predelay effect ������ ���� �־��ش�.
{
    protected Dictionary<EffectCondition, EffectData> m_effectDatas;
    protected Dictionary<EffectCondition, SoundData> m_soundDatas;

    List<BasicEffectPlayer> m_effectplayers = new List<BasicEffectPlayer>();
    List<SoundPlayer> m_soundPlayers = new List<SoundPlayer>();

    public EffectSpawnComponent(Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
    {
        m_effectDatas = effectDatas;
        m_soundDatas = soundDatas;
    }

    public void PlaySound(Transform target, SoundData data)
    {
        SoundManager.instance.PlaySFX(target.position, data.name, data.volume);
    }

    /// <summary>
    /// ���� ����ٴ�
    /// </summary>
    public bool PlayEffect(Transform target, EffectCondition condition, float scale = 1)
    {
        if (target == null || m_effectDatas.Count == 0 || m_effectDatas.ContainsKey(condition) == false) return false;

        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(m_effectDatas[condition].name); // ����Ʈ �̸� �߰�
        if (player == null) return false;

        // bool ������ �Ǵ��ϱ�
        if(m_effectDatas[condition].isFix) player.Init(target, m_effectDatas[condition].disableTime, scale);
        else player.Init(target.position, m_effectDatas[condition].disableTime, scale);

        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    /// <summary>
    /// ��ġ�� ����
    /// </summary>
    public bool PlayEffect(Vector3 targetPos, EffectCondition condition, float scale = 1)
    {
        if (m_effectDatas.Count == 0 || m_effectDatas.ContainsKey(condition) == false) return false;

        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(m_effectDatas[condition].name); // ����Ʈ �̸� �߰�
        if (player == null) return false;

        player.Init(targetPos, m_effectDatas[condition].disableTime, scale);
        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    public bool PlayEffect(Vector3 posVec, List<Vector3> pos, EffectCondition condition)
    {
        if (m_effectDatas.Count == 0 || m_effectDatas.ContainsKey(condition) == false) return false;

        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(m_effectDatas[condition].name); // ����Ʈ �̸� �߰�
        if (player == null) return false;

        player.Init(posVec, m_effectDatas[condition].disableTime, pos);
        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    public void StopEffect()
    {
        for (int i = 0; i < m_effectplayers.Count; i++)
        {
            m_effectplayers[i].StopEffect();
        }

        m_effectplayers.Clear();
    }

    public void StopSound()
    {
        for (int i = 0; i < m_soundPlayers.Count; i++)
        {
            m_soundPlayers[i].gameObject.SetActive(false);
        }

        m_soundPlayers.Clear();
    }
}

abstract public class BaseMethod<T> : EffectSpawnComponent
{
    public BaseMethod(Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(effectDatas, soundDatas)
    {
    }

    //public abstract void AddState(SkillData data); // ������ ���̽����� Stat�� ī���ؼ� ������

    public virtual void Execute(SkillSupportData supportData, T target) { }

    public virtual void Execute(SkillSupportData supportData) { }
}

[System.Serializable]
public struct EffectData
{
    public string name;
    public bool isFix;
    public float disableTime;
}

[System.Serializable]
public struct SoundData
{
    public string name;
    public bool isLoop;
    public float volume;
}

[System.Serializable]
abstract public class BaseSkill
{
    /// <summary>
    /// ��ų�� ������ �� ����
    /// </summary>
    public enum OverlapType { None, Restart, CountUp } // �� �ٸ� ��ų�� ����Ʈ�� �־ ���, ������ �����ϴ� ��ų�� �ٽ� �����ؼ� ���

    /// <summary>
    /// ��ų ��� ����, InRange, OutRange�� ���� ���� �� �÷��̾ ���� ������ ���Դ��� �ƴ��� üũ��
    /// </summary>
    public enum UseConditionType { Contact, Get, InRange, OutRange, Init, End }

    protected GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }

    protected string m_name;
    public string Name { get { return m_name; } }

    protected bool m_isRunning = false;
    public bool IsRunning { get { return m_isRunning; } }

    protected int m_useCount;

    protected float m_duration;
    protected int m_tickCount;
    protected float m_preDelay;

    protected int m_useCountDownPoint = 1; // 0 �Ǵ� 1�� ���� ����

    protected int m_useCountUpPoint = 1; // 0 �Ǵ� 1�� ���� ����


    protected bool m_canFinish = false;

    protected bool m_nowFinish = false;
    public bool NowFinish { get { return m_nowFinish; } }

    UseConditionType m_useConditionType;
    public UseConditionType UseCondition { get { return m_useConditionType; } }

    OverlapType m_overlapType;
    public OverlapType OverlapCondition { get { return m_overlapType; } }

    public void CountUp()
    {
        if(m_canFinish) m_useCount += m_useCountUpPoint;
    }
    public void CountDown()
    {
        if (m_canFinish) m_useCount -= m_useCountDownPoint;
    }

    protected bool CanFinish
    {
        get
        {
            if (m_useCount == 0 && m_canFinish) return true; // ���Ƚ���� 0�̰ų� canFinish�� true�� ���
            else return false;
        }
    }

    public bool IsUseCountZero()
    {
        return m_useCount == 0;
    }

    public BaseSkill(string name, UseConditionType useConditionType, OverlapType overlapType, bool canFinish, float duration, int tickCount, float preDelay, int useCount)
    {
        m_name = name;
        m_useConditionType = useConditionType;
        m_overlapType = overlapType;
        m_duration = duration;
        m_tickCount = tickCount;
        m_preDelay = preDelay;
        m_useCount = useCount;

        m_canFinish = canFinish;
    }

    public abstract void Init(GameObject caster);
    public abstract void Execute(); // ��ų ����
    public abstract void End(); // ���� �� ���� ����
    public abstract void Reset(); // ��ų �� ���� ���
}


public class Skill<T> : BaseSkill // ���� ���� ����
{
    public Skill(string name, UseConditionType useConditionType, OverlapType overlapType, bool canFinish, float duration, int tickCount, float preDelay, int useCount) 
        : base(name, useConditionType, overlapType, canFinish, duration, tickCount, preDelay, useCount) // tickCount�� 1�� ���� �����Ű��
    {
    }

    protected EffectSpawnComponent m_predelayEffectSpawner;

    protected SpecifyLocation m_specifyLocation;

    protected TargetDesignation<T> m_targetDesignation;

    protected List<BaseMethod<T>> m_baseMethods;

    float m_storedPreDelay = 0;
    int m_storedTickCount = 0;
    float m_storedDuration = 0;

    bool canExecuteSkillRoutine = true;

    public virtual void SkillRoutine(int tickCount)
    {
        Vector3 myPos = m_specifyLocation.ReturnPos();

        SkillSupportData supportData = new SkillSupportData(m_caster, myPos, tickCount);

        T target = m_targetDesignation.Execute(supportData);

        if(target == null)
        {
            for (int i = 0; i < m_baseMethods.Count; i++)
            {
                m_baseMethods[i].Execute(supportData);
            }

            return;
        }

        for (int i = 0; i < m_baseMethods.Count; i++)
        {
            m_baseMethods[i].Execute(supportData, target);
        }
    }

    public override void Init(GameObject caster) // fix ���� �־��ֱ�
    {
        m_caster = caster;
        m_specifyLocation.Init(caster, out Transform target, out float scale); // IsFix�� data ������ ����, target�� �����ؼ� ����ش�. �̰� �������� ���� ����Ʈ �����ϱ� 

        m_predelayEffectSpawner.PlayEffect(target, EffectCondition.PredelayEffect, scale);
        // --> scale�� x, y, z �� �ϳ��� ����ؼ� ����Ʈ ������ ����
    }

    public override void Execute()
    {
        m_isRunning = true;

        if (m_storedPreDelay < m_preDelay)
        {
            // Predelay Effect �����غ��� --> ���⼭ PreDelay Effect ���
            // ""�̸� ��� �� �ϴ� �ɷ�
            m_storedPreDelay += Time.deltaTime;
            return;
        }

        if(m_duration == 0 && m_tickCount == 0) // burst ��ų --> ƽ ī��Ʈ�� ��� �Ⱓ ��� 0
        {
            SkillRoutine(m_storedTickCount);
            AfterUse();
        }
        else // tick ��ų
        {
            if (m_tickCount > m_storedTickCount)
            {
                if(canExecuteSkillRoutine) SkillRoutine(m_storedTickCount);

                canExecuteSkillRoutine = false;

                m_storedDuration += Time.deltaTime;
                float tickDuration = m_duration / m_tickCount;
                if (m_storedDuration > tickDuration)
                {
                    canExecuteSkillRoutine = true;
                    m_storedDuration = 0;
                    m_storedTickCount += 1;
                }
            }
            else
            {
                AfterUse();
            }
        }
    }

    void AfterUse()
    {
        m_isRunning = false;
        CountDown();

        //StopMethodEffects();

        if (CanFinish == true)
        {
            m_nowFinish = true;
        }
        else
        {
            Reset();
        }
    }

    void StopMethodEffects()
    {
        for (int i = 0; i < m_baseMethods.Count; i++)
        {
            m_baseMethods[i].StopEffect();
        }
    }

    public override void End()
    {
        // ���� ������
        Reset();
        //if (!m_canFinish) return; // ���ŵ��� �ʴ� ��ų�� ��� ����



        m_nowFinish = false;
    }

    public override void Reset()
    {
        // ���� ���� �ʱ�ȭ
        m_storedPreDelay = 0;
        m_storedTickCount = 0;
        m_storedDuration = 0;
    }
}

[System.Serializable]
public class CasterCircleRangeAttack : Skill<RaycastHit2D[]>
{
    // ���⿡ ���� �������ֱ� X --> ������� ���� ������Ʈ�� �ٷ� �ʱ�ȭ�ع�����


    // ���� ����Ʈ liftTime�� preDelay�� �����ش�.

    //float[] skillScalePerTicks;
    // ����Ʈ�� �̿� ������ ����
    // Ex) ��ų ����, ����Ʈ ������ 1 : 1 ���� ������
    // ���� ��ų ������ Ŀ���� �׿� ���� ����Ʈ�� Ű��� ����� ����

    // effectDuration�� ����Ʈ ���� ��, ������ֱ�

    // targetFindRange�� Ÿ���� ã��
    // 

    // Effect�� Sound�� �и��ؼ� �����Ű�� --> ���뼺�� �ʹ� �������� ���� �׷��� �� �ʿ䰡 ����


    // json���� �ҷ����� ����̹Ƿ� ������ ���� Ŭ���� ���� �ۼ��������, ���� ������ ������ �ʱ�ȭ���ִ� �ڵ带 �߰��� ����

    public CasterCircleRangeAttack(string name, UseConditionType useConditionType, OverlapType overlapType, bool canFinish, int useCount, bool isFix, float duration, int tickCount, float preDelay, float targetFindRange, float[] skillScalePerTicks,
        Vector2[] offsetRangePerTick, EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas) 
        : base(name, useConditionType, overlapType, canFinish, duration, tickCount, preDelay, useCount)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(isFix);
        m_targetDesignation = new FindInCircleRange(targetFindRange, skillScalePerTicks, offsetRangePerTick);
        m_baseMethods = new List<BaseMethod<RaycastHit2D[]>> { new DamageUsingRaycast(hitTarget, knockBackThrust, damage, skillScalePerTicks, true, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class ContactorCircleRangeAttack : Skill<RaycastHit2D[]>
{
    public ContactorCircleRangeAttack(string name, UseConditionType useConditionType, OverlapType overlapType, bool canFinish, int useCount, bool isFix, float duration, int tickCount, float preDelay, float targetFindRange, float[] skillScalePerTicks,
        Vector2[] offsetRangePerTick, EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, overlapType, canFinish, duration, tickCount, preDelay, useCount)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToContactor(isFix, hitTarget);
        m_targetDesignation = new FindInCircleRange(targetFindRange, skillScalePerTicks, offsetRangePerTick);
        m_baseMethods = new List<BaseMethod<RaycastHit2D[]>> { new DamageUsingRaycast(hitTarget, knockBackThrust, damage, skillScalePerTicks, false, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class ContactedAttack : Skill<List<ContactData>>
{
    public ContactedAttack(string name, UseConditionType useConditionType, EntityTag[] hitTarget, float knockBackThrust, float damage,
        EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, OverlapType.None, false, 0, 1, 0, 1)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(false);
        m_targetDesignation = new FindInContacted();
        m_baseMethods = new List<BaseMethod<List<ContactData>>> { new DamageToContactors(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class CasterBoxRangeAttack : Skill<RaycastHit2D[]>
{

    public CasterBoxRangeAttack(string name, UseConditionType useConditionType, OverlapType overlapType, bool canFinish, int useCount, bool isFix, float duration, int tickCount, float preDelay, Vector2[] boxRangePerTick, Vector2[] offsetRangePerTick, float[] skillScalePerTicks,
        EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, overlapType, canFinish, duration, tickCount, preDelay, useCount)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(isFix);
        m_targetDesignation = new FindInBoxRange(boxRangePerTick, offsetRangePerTick);
        m_baseMethods = new List<BaseMethod<RaycastHit2D[]>> { new DamageUsingRaycast(hitTarget, knockBackThrust, damage, skillScalePerTicks, true, effectDatas, soundDatas) };
    }
}


[System.Serializable]
public class CastBuffToPlayer : Skill<Transform>
{
    public CastBuffToPlayer(string name, UseConditionType useConditionType, EntityTag[] hitTarget, bool nowApply, string[] buffNames,
        EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, OverlapType.None, false, 0, 1, 0, 1)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(false);
        m_targetDesignation = new FindPlayer();
        m_baseMethods = new List<BaseMethod<Transform>> { new BuffToTarget(hitTarget, nowApply, buffNames, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class CastBuffToCaster : Skill<GameObject>
{
    public CastBuffToCaster(string name, UseConditionType useConditionType, EntityTag[] hitTarget, bool nowApply, string[] buffNames,
        EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, OverlapType.None, true, 0, 1, 0, 1)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(false);
        m_targetDesignation = new NoFound();
        m_baseMethods = new List<BaseMethod<GameObject>> { new BuffToCaster(hitTarget, nowApply, buffNames, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class SpawnRotationBallAround : Skill<GameObject>
{
    public SpawnRotationBallAround(string name, UseConditionType useConditionType, string projectileName, float duration, int tickCount, int projectileCount, float preDelay,
        float distanceFromCaster, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, OverlapType.Restart, false, duration, tickCount, preDelay, 1) // ���ᰡ �� �ǰ��ϰ� ���߿� ��� ������Ʈ �ı��� �� ���� �����Ű�� �ɷ�
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(true);
        m_targetDesignation = new NoFound();
        m_baseMethods = new List<BaseMethod<GameObject>> { new SpawnRotationBall(projectileName, projectileCount, distanceFromCaster, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class ShootBulletInCircleRange : Skill<GameObject>
{
    public ShootBulletInCircleRange(string name, UseConditionType useConditionType, string projectileName, float duration, int tickCount, int projectileCount, float preDelay,
        float speed, bool isClockwise, float distanceFromCaster, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, OverlapType.None, false, duration, tickCount, preDelay, 1)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(true);
        m_targetDesignation = new NoFound();
        m_baseMethods = new List<BaseMethod<GameObject>> { new SpawnBulletInCircleRange(projectileName, projectileCount, speed, isClockwise, distanceFromCaster, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class ShootLaserToRandomDirection : Skill<RaycastHit2D[]>
{
    public ShootLaserToRandomDirection(string name, UseConditionType useConditionType, OverlapType overlapType, bool canFinish, int useCount, float duration, int tickCount, float preDelay, float[] rangePerTicks,
        EntityTag[] hitTarget, float knockBackThrust, float damage, float laserMaxDistance, EntityTag[] blockedTag, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, overlapType, canFinish, duration, tickCount, preDelay, useCount)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(true);
        m_targetDesignation = new FindAllUsingRaycast(rangePerTicks);
        m_baseMethods = new List<BaseMethod<RaycastHit2D[]>> { new DamageUsingLaserHit(hitTarget, knockBackThrust, damage, effectDatas, soundDatas, laserMaxDistance, blockedTag) };
    }
}

[System.Serializable]
public class ShootSpawnedObject : Skill<GameObject>
{
    public ShootSpawnedObject(string name, UseConditionType useConditionType, string projectileName, float duration, int tickCount, float preDelay,
        float speed, bool isNeedCaster, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, OverlapType.None, true, duration, tickCount, preDelay, 1) 
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(false);
        m_targetDesignation = new NoFound();
        m_baseMethods = new List<BaseMethod<GameObject>> { new SpawnAndShootProjectile(projectileName, speed, isNeedCaster, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class SpawnObject : Skill<GameObject>
{
    public SpawnObject(string name, UseConditionType useConditionType, string projectileName, float duration, int tickCount, float preDelay,
        bool isNeedCaster, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name, useConditionType, OverlapType.None, true, duration, tickCount, preDelay, 1)
    {
        m_predelayEffectSpawner = new EffectSpawnComponent(effectDatas, soundDatas);
        m_specifyLocation = new LocationToCaster(false);
        m_targetDesignation = new NoFound();
        m_baseMethods = new List<BaseMethod<GameObject>> { new SpawnProjectile(projectileName, isNeedCaster, effectDatas, soundDatas) };
    }
}