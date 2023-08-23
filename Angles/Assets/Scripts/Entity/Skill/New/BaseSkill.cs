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
//        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
//        if (player == null) return false;

//        player.IsFixed = true;
//        player.AddState(target, effectData.disableTime);
//        player.PlayEffect();


//        if(effectData.soundName != null)
//            SoundManager.instance.PlaySFX(target.position, effectData.soundName, effectData.volume);

//        effectPlayer = player;
//        return true;
//    }

//    public bool Play(Vector3 pos)
//    {
//        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
//        if (player == null) return false;

//        player.IsFixed = false;
//        player.AddState(pos, effectData.disableTime);
//        player.PlayEffect();

//        if (effectData.soundName != null)
//            SoundManager.instance.PlaySFX(pos, effectData.soundName, effectData.volume);

//        effectPlayer = player;
//        return true;
//    }

//    public void Stop()
//    {
//        effectPlayer.StopEffect();
//        effectPlayer = null;
//    }
//}

abstract public class BaseMethod<T>
{
    protected Dictionary<EffectCondition, EffectData> m_effectDatas;
    protected Dictionary<EffectCondition, SoundData> m_soundDatas;

    List<BasicEffectPlayer> m_effectplayers;
    List<SoundPlayer> m_soundPlayers;

    // --> 스킬 사용 끝나면 만약 남아있는 Player를 중지시켜준다.

    public BaseMethod(Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
    {
        m_effectDatas = effectDatas;
        m_soundDatas = soundDatas;
    }

    //public abstract void AddState(SkillData data); // 데이터 베이스에서 Stat을 카피해서 가져옴

    public virtual void Execute(SkillSupportData supportData, T target) { }

    public virtual void Execute(SkillSupportData supportData) { }

    protected void PlaySound(Transform target, SoundData data)
    {
        SoundManager.instance.PlaySFX(target.position, data.name, data.volume);
    }

    protected bool PlayEffect(Transform target, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.Init(target, data.disableTime);
        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    protected bool PlayEffect(Vector3 targetPos, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.Init(targetPos, data.disableTime);
        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    protected void StopEffect()
    {
        for (int i = 0; i < m_effectplayers.Count; i++)
        {
            m_effectplayers[i].StopEffect();
        }

        m_effectplayers.Clear();
    }
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
    public enum UseConditionType { Contact, Get, InRange, OutRange, Init }

    protected GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }

    protected string m_name;
    public string Name { get { return m_name; } }

    protected bool m_isRunning;
    public bool IsRunning { get { return m_isRunning; } }

    protected int m_useCount;

    protected float m_duration;
    protected int m_tickCount;
    protected float m_preDelay;

    protected string m_preDelayEffectName; // null이면 이팩트 재생 X

    protected int m_useCountDownPoint; // 0 또는 1로 만들어서 적용

    protected int m_useCountUpPoint; // 0 또는 1로 만들어서 적용

    protected bool m_nowFinish = false;
    public bool NowFinish { get { return m_nowFinish; } }

    UseConditionType m_useConditionType;
    public UseConditionType UseCondition { get { return m_useConditionType; } }

    OverlapType m_overlapType;
    public OverlapType OverlapCondition { get { return m_overlapType; } }

    public void CountUp() => m_useCount += m_useCountUpPoint;
    public void CountDown() => m_useCount -= m_useCountDownPoint;

    protected bool IsZeroCount
    {
        get
        {
            if (m_useCount == 0) return true;
            else return false;
        }
    }

    public bool IsUseCountZero()
    {
        return m_useCount == 0;
    }

    public BaseSkill(string name, float duration, int tickCount, float preDelay, string preDelayEffectName)
    {
        m_name = name;
        m_duration = duration;
        m_tickCount = tickCount;
        m_preDelay = preDelay;
        m_preDelayEffectName = preDelayEffectName;
    }

    public abstract void Init(GameObject caster);
    public abstract void Execute(); // 스킬 실행
    public abstract void End(); // 종료 시 변수 리셋
    public abstract void Reset(); // 스킬 재 사용시 사용
}


public class Skill<T> : BaseSkill // 실제 구현 포함
{
    public Skill(string name, float duration = 0, int tickCount = 1, float preDelay = 0, string preDelayEffectName = null) : base(name, duration, tickCount, preDelay, preDelayEffectName)
    {
    }

    [JsonProperty]
    protected SpecifyLocation m_specifyLocation;

    [JsonProperty]
    protected TargetDesignation<T> m_targetDesignation;

    [JsonProperty]
    protected List<BaseMethod<T>> m_baseMethods;

    float m_storedPreDelay = 0;
    int m_storedTickCount = 0;
    float m_storedDuration = 0;

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
        m_specifyLocation.Init(caster); // IsFix는 data 변수로 지정
    }

    public override void Execute()
    {
        m_isRunning = true;

        float tickDuration = m_duration / m_tickCount;
        if (m_storedPreDelay >= m_preDelay)
        {
            // Predelay Effect 제작해보자
            m_storedPreDelay += Time.deltaTime;
            return;
        }

        if (m_tickCount > m_storedTickCount)
        {
            SkillRoutine(m_storedTickCount);

            m_storedDuration += Time.deltaTime;
            if (m_storedDuration > tickDuration)
            {
                m_storedDuration = 0;
                m_storedTickCount += 1;
            }
            else
            {
                m_isRunning = false;
                CountDown();

                if(IsZeroCount == true)
                {
                    m_nowFinish = true;
                }
                else
                {
                    Reset();
                }
            }
        }
    }

    public override void End()
    {
        // 종료 시퀀스
        Reset();
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

    public CasterCircleRangeAttack(string name, bool isFix, float duration, int tickCount, float preDelay, float targetFindRange, float[] skillScalePerTicks,
        EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas, string preDelayEffectName = null) 
        : base(name, duration, tickCount, preDelay, preDelayEffectName)
    {
        bool isTickPerRangeSame;

        if (skillScalePerTicks.Length == 0) isTickPerRangeSame = true;
        else isTickPerRangeSame = false;

        m_specifyLocation = new LocationToContactor(isFix);
        m_targetDesignation = new FindInCircleRange(isTickPerRangeSame, targetFindRange, skillScalePerTicks);
        m_baseMethods = new List<BaseMethod<RaycastHit2D[]>> { new DamageToRaycastHit(hitTarget, knockBackThrust, damage, skillScalePerTicks, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class ContactedAttack : Skill<List<ContactData>>
{
    public ContactedAttack(string name, EntityTag[] hitTarget, float knockBackThrust, float damage, 
        EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas)
        : base(name)
    {
        m_specifyLocation = new LocationToContactor(false);
        m_targetDesignation = new FindInContacted();
        m_baseMethods = new List<BaseMethod<List<ContactData>>> { new DamageToContactors(hitTarget, knockBackThrust, damage, effectDatas, soundDatas) };
    }
}

[System.Serializable]
public class CasterBoxRangeAttack : Skill<RaycastHit2D[]>
{

    public CasterBoxRangeAttack(string name, bool isFix, float duration, int tickCount, float preDelay, Vector2[] boxRangePerTick, Vector2[] offsetRangePerTick, float[] skillScalePerTicks,
        EntityTag[] hitTarget, float knockBackThrust, float damage, EffectConditionEffectDataDictionary effectDatas, EffectConditionSoundDataDictionary soundDatas, string preDelayEffectName = null)
        : base(name, duration, tickCount, preDelay, preDelayEffectName)
    {
        bool isTickPerRangeSame;

        if (skillScalePerTicks.Length == 0) isTickPerRangeSame = true;
        else isTickPerRangeSame = false;

        m_specifyLocation = new LocationToContactor(isFix);
        m_targetDesignation = new FindInBoxRange(isTickPerRangeSame, boxRangePerTick, offsetRangePerTick);
        m_baseMethods = new List<BaseMethod<RaycastHit2D[]>> { new DamageToRaycastHit(hitTarget, knockBackThrust, damage, skillScalePerTicks, effectDatas, soundDatas) };
    }
}