using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public struct SkillSupportData // --> 추후에 버프 추가
{
    public SkillSupportData(GameObject caster, Vector3 pos, int tickCount)//, int tickCount)
    {
        m_caster = caster;
        m_Pos = pos;
        m_tickCount = tickCount;
    }

    /// <summary>
    /// 스킬 시전자
    /// </summary>
    GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }



    /// <summary>
    /// 스킬이 시전될 위치
    /// </summary>
    Vector3 m_Pos;
    public Vector3 Pos { get { return m_Pos; } }

    /// <summary>
    /// 현재 TickCount
    /// </summary>
    int m_tickCount;
    public int TickCount { get { return m_tickCount; } }
}

public enum EffectCondition // 맞을 때, 표면에 생기는 이팩트, 스킬 자체 이팩트 등등 특정 조건에 명명할 단어를 넣어놓는다.
{
    PredelayEffect, // 스킬 시전 사전 이펙트

    HitSurfaceEffect, // 데미지를 입은 표면에 일어나는 효과

    AttackEffect, // 공격 스킬을 사용했을 때 나오는 효과

    BuffEffect, // 버프가 걸릴 때 나오는 효과

    SpawnEffect // 스폰이 되었을 때 나오는 효과
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
//// 목표: Effect와 Sound를 같이 재생해준다.
//public class AudioVisualPlayer
//{
//    BasicEffectPlayer effectPlayer;
//    SoundPlayer soundPlayer;
//    AudioVisualData data;

//    public EffectSource(AudioVisualData effectData) // 사운드 플레이 이밴트 트리거, 이팩트 데이터
//    {
//        this.effectData = effectData;
//    }

//    public bool Play(Transform target)
//    {
//        BasicEffectPlayer playerTransform = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
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
//        BasicEffectPlayer playerTransform = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
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

public class EffectSpawnComponent // --> 이거를 BaseMethod에 상속시켜서 스폰 ㄱㄱ + 추가로 스킬 클래스에도 predelay effect 스폰을 위해 넣어준다.
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
    /// 직접 따라다님
    /// </summary>
    public bool PlayEffect(Transform target, EffectCondition condition, float scale = 1)
    {
        if (target == null || m_effectDatas.Count == 0 || m_effectDatas.ContainsKey(condition) == false) return false;

        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(m_effectDatas[condition].name); // 이팩트 이름 추가
        if (player == null) return false;

        // bool 값으로 판단하기
        if(m_effectDatas[condition].isFix) player.Init(target, m_effectDatas[condition].disableTime, scale);
        else player.Init(target.position, m_effectDatas[condition].disableTime, scale);

        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    /// <summary>
    /// 위치만 지정
    /// </summary>
    public bool PlayEffect(Vector3 targetPos, EffectCondition condition, float scale = 1)
    {
        if (m_effectDatas.Count == 0 || m_effectDatas.ContainsKey(condition) == false) return false;

        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(m_effectDatas[condition].name); // 이팩트 이름 추가
        if (player == null) return false;

        player.Init(targetPos, m_effectDatas[condition].disableTime, scale);
        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    public bool PlayEffect(Vector3 posVec, List<Vector3> pos, EffectCondition condition)
    {
        if (m_effectDatas.Count == 0 || m_effectDatas.ContainsKey(condition) == false) return false;

        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(m_effectDatas[condition].name); // 이팩트 이름 추가
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

    //public abstract void AddState(SkillData data); // 데이터 베이스에서 Stat을 카피해서 가져옴

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
    /// 스킬을 재사용할 지 여부
    /// </summary>
    public enum OverlapType { None, Restart, CountUp } // 또 다른 스킬을 리스트에 넣어서 사용, 기존에 존재하던 스킬을 다시 리셋해서 사용

    /// <summary>
    /// 스킬 사용 조건, InRange, OutRange는 적이 추적 중 플레이어가 공격 범위에 들어왔는지 아닌지 체크함
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

    protected int m_useCountDownPoint = 1; // 0 또는 1로 만들어서 적용

    protected int m_useCountUpPoint = 1; // 0 또는 1로 만들어서 적용


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
            if (m_useCount == 0 && m_canFinish) return true; // 사용횟수가 0이거나 canFinish가 true의 경우
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
    public abstract void Execute(); // 스킬 실행
    public abstract void End(); // 종료 시 변수 리셋
    public abstract void Reset(); // 스킬 재 사용시 사용
}


public class Skill<T> : BaseSkill // 실제 구현 포함
{
    public Skill(string name, UseConditionType useConditionType, OverlapType overlapType, bool canFinish, float duration, int tickCount, float preDelay, int useCount) 
        : base(name, useConditionType, overlapType, canFinish, duration, tickCount, preDelay, useCount) // tickCount는 1을 빼고 적용시키기
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

    public override void Init(GameObject caster) // fix 변수 넣어주기
    {
        m_caster = caster;
        m_specifyLocation.Init(caster, out Transform target, out float scale); // IsFix는 data 변수로 지정, target을 지정해서 념겨준다. 이걸 바탕으로 사전 이팩트 지정하기 

        m_predelayEffectSpawner.PlayEffect(target, EffectCondition.PredelayEffect, scale);
        // --> scale의 x, y, z 중 하나만 사용해서 이팩트 스케일 조정
    }

    public override void Execute()
    {
        m_isRunning = true;

        if (m_storedPreDelay < m_preDelay)
        {
            // Predelay Effect 제작해보자 --> 여기서 PreDelay Effect 출력
            // ""이면 출력 안 하는 걸로
            m_storedPreDelay += Time.deltaTime;
            return;
        }

        if(m_duration == 0 && m_tickCount == 0) // burst 스킬 --> 틱 카운트랑 사용 기간 모두 0
        {
            SkillRoutine(m_storedTickCount);
            AfterUse();
        }
        else // tick 스킬
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
        // 종료 시퀀스
        Reset();
        //if (!m_canFinish) return; // 제거되지 않는 스킬의 경우 재사용



        m_nowFinish = false;
    }

    public override void Reset()
    {
        // 기존 변수 초기화
        m_storedPreDelay = 0;
        m_storedTickCount = 0;
        m_storedDuration = 0;
    }
}

[System.Serializable]
public class CasterCircleRangeAttack : Skill<RaycastHit2D[]>
{
    // 여기에 변수 선언해주기 X --> 선언없이 하위 컴포넌트를 바로 초기화해버리자


    // 사전 이팩트 liftTime은 preDelay로 맞춰준다.

    //float[] skillScalePerTicks;
    // 이팩트로 이에 영향을 받음
    // Ex) 스킬 범위, 이팩트 범위는 1 : 1 대응 관계임
    // 따라서 스킬 범위가 커지면 그에 맞춰 이팩트를 키우는 방식을 취함

    // effectDuration은 이팩트 만들 때, 만들어주기

    // targetFindRange로 타겟을 찾음
    // 

    // Effect와 Sound는 분리해서 재생시키자 --> 범용성도 너무 떨어지고 굳이 그렇게 할 필요가 없음


    // json으로 불러오는 방식이므로 변수를 따로 클레스 내에 작성해줘야함, 이후 생성자 내에서 초기화해주는 코드를 추가로 넣자

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
        : base(name, useConditionType, OverlapType.Restart, false, duration, tickCount, preDelay, 1) // 종료가 안 되게하고 나중에 모든 오브젝트 파괴될 때 따로 종료시키는 걸로
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