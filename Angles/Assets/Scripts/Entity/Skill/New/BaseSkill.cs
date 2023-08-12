using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

//public struct DamageSupportData
//{
//    public string name; // --> 데이터 베이스에서 스킬 이름을 바탕(현 prefabData)으로 해당 데이터를 찾아서 넣어줌
//    public Dictionary<EffectCondition, EffectData> effectDatas;

//    //public List<float> attackCircleRange; // 틱마다 범위가 달라짐

//}

//public struct BuffSupportData
//{
//    public string name; // --> 데이터 베이스에서 스킬 이름을 바탕(현 prefabData)으로 해당 데이터를 찾아서 넣어줌
//    public Dictionary<EffectCondition, EffectData> effectDatas;
//    public List<string> buffNames;
//    public bool nowApply; // 버프를 적용할 건지 제거할건지 확인

//    //public List<float> attackCircleRange; // 틱마다 범위가 달라짐

//}
//public struct SpawnSupportData
//{
//    public string name; // --> 데이터 베이스에서 스킬 이름을 바탕(현 prefabData)으로 해당 데이터를 찾아서 넣어줌

//    public Dictionary<EffectCondition, EffectData> effectDatas;
//    public float speed;
//    public float distanceFromCaster;

//    public string projectileName;
//    public int projectileCount;
//}

public enum EffectCondition // 맞을 때, 표면에 생기는 이팩트, 스킬 자체 이팩트 등등 특정 조건에 명명할 단어를 넣어놓는다.
{
    HitSurfaceEffect, // 데미지를 입은 표면에 일어나는 효과

    AttackEffect, // 공격 스킬을 사용했을 때 나오는 효과

    BuffEffect, // 버프가 걸릴 때 나오는 효과

    SpawnEffect // 스폰이 되었을 때 나오는 효과
}

public struct EffectData // 이팩트 시간, 크기 등등 조작 가능하게끔 재현
{
    public string name;
    public float duration;
    public string soundName;
    public float volume;
}

public class EffectSource
{
    EffectData effectData;
    BasicEffectPlayer effectPlayer;

    public EffectSource(EffectData data) // 사운드 플레이 이밴트 트리거, 이팩트 데이터
    {
        effectData = data;
    }

    public bool Play(Transform target)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = true;
        player.Init(target, effectData.duration);
        player.PlayEffect();

        SoundManager.instance.PlaySFX(target.position, effectData.soundName, effectData.volume);

        effectPlayer = player;
        return true;
    }

    public bool Play(Vector3 pos)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = false;
        player.Init(pos, effectData.duration);
        player.PlayEffect();

        SoundManager.instance.PlaySFX(pos, effectData.soundName, effectData.volume);

        effectPlayer = player;
        return true;
    }

    public void Stop()
    {
        effectPlayer.StopEffect();
        effectPlayer = null;
    }
}

abstract public class BaseMethod<T>
{
    protected Dictionary<EffectCondition, EffectData> m_effectDatas;
    List<EffectSource> m_effectSources;

    public BaseMethod(Dictionary<EffectCondition, EffectData> effectDatas)
    {
        m_effectDatas = effectDatas;
    }

    //public abstract void Init(SkillData data); // 데이터 베이스에서 Stat을 카피해서 가져옴

    public virtual void Execute(SkillSupportData supportData, T target) { }

    public virtual void Execute(SkillSupportData supportData) { }

    protected bool PlayEffect(Transform target, EffectData data)
    {
        EffectSource effectSource = new EffectSource(data);
        effectSource.Play(target);

        m_effectSources.Add(effectSource);
        return true;
    }

    protected bool PlayEffect(Vector3 pos, EffectData data)
    {
        EffectSource effectSource = new EffectSource(data);
        effectSource.Play(pos);

        m_effectSources.Add(effectSource);
        return true;
    }

    protected void StopEffect()
    {
        for (int i = 0; i < m_effectSources.Count; i++)
        {
            m_effectSources[i].Stop();
        }

        m_effectSources.Clear();
    }
}

public interface ISkill
{
    public bool CheckIsFinish();

    public void Init(Transform caster, SkillData data);
    public void Execute(); // 스킬 실행

    public void Finish(); // 스킬 사용 종료

    public void End(); // 종료 시 변수 리셋
    public void Reset(); // 스킬 재 사용시 사용

    public ISkill CreateCopy();
}

public class BaseSkill<T> : ISkill
{
    public ISkill CreateCopy()
    {
        return new BaseSkill<T>();
    }

    public BaseSkill() // SpecifyLocation specifyLocation, TargetDesignation<T> targetDesignation, List<BaseMethod<T>> methods
    {
        //m_specifyLocation = specifyLocation;
        //m_targetDesignation = targetDesignation;
        //m_baseMethods = methods;
    }

    //[SerializeField]
    //protected SkillData m_data; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    //public SkillData Data { get { return m_data; } }


    // 변수 선언


    //[SerializeField]
    //EntityTag[] hitTarget;
    //public EntityTag[] HitTarget { get { return hitTarget; } set { hitTarget = value; } }

    // 이런 변수는 하위 클레스에서 선언



    int tickCount;

    float duration;




    // 변수 선언



    protected SpecifyLocation m_specifyLocation;
    public SpecifyLocation SpecifyLocation { get { return m_specifyLocation; } }

    protected TargetDesignation<T> m_targetDesignation;
    public TargetDesignation<T> TargetDesignation { get { return m_targetDesignation; } }

    [SerializeField]
    protected List<BaseMethod<T>> m_baseMethods;

    float m_preDelay = 0;

    int m_tickCount = 0;

    float m_storedDuration = 0;

    bool m_nowFinish = false;
    public bool NowFinish { get { return m_nowFinish; } }

    

    public virtual void Init(Transform caster, SkillData data)
    {
        m_data = data;
        m_specifyLocation.Init(caster, true); // IsFix는 data 변수로 지정

        for (int i = 0; i < m_baseMethods.Count; i++) // 가져와야할 스탯이 여러 개인 경우, 여기서 인덱스를 넘겨서 호출시키기
        {
            m_baseMethods[i].Init(data); // 데이터를 담은 SO를 호출해서 초기화시켜줌
        }
    }

    public virtual void Execute() // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        float tickDuration = m_data.Duration / m_data.TickCount;
        if (m_data.PreDelay >= m_preDelay)
        {
            m_preDelay += Time.deltaTime;
            return;
        }

        if (m_data.TickCount > m_tickCount)
        {
            SkillRoutine(m_data.TickCount);

            m_storedDuration += Time.deltaTime;
            if (m_storedDuration > tickDuration)
            {
                m_storedDuration = 0;
                m_tickCount += 1;
            }
            else
            {
                m_nowFinish = true;
            }
        }
    }

    public virtual void SkillRoutine(int tickCount)
    {
        Vector3 myPos = m_specifyLocation.ReturnPos();

        SkillSupportData supportData = new SkillSupportData(m_specifyLocation.Caster, myPos, tickCount);

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

    public virtual void Reset()
    {
        m_preDelay = 0;
        m_tickCount = 0;
        m_storedDuration = 0;
    }

    public virtual void End()
    {
        // 종료 시퀀스
        Reset();
        m_nowFinish = false;
    }

    public virtual void Finish()
    {
        m_nowFinish = true;
    }

    public bool CheckIsFinish()
    {
        return NowFinish;
    }
}

public class CasterCircleRangeAttack : BaseSkill<RaycastHit2D[]>
{
    // 여기에 변수 선언해주기 X --> 선언없이 하위 컴포넌트를 바로 초기화해버리자

    public CasterCircleRangeAttack(List<float> radiusRangePerTick) // SO를 만들어서 데이터를 불러오기 
    {
        m_specifyLocation = new LocationToContactor();
        m_targetDesignation = new FindInCircleRange(radiusRangePerTick);
        m_baseMethods = new List<BaseMethod<RaycastHit2D[]>> { new DamageToRaycastHit() };
    }
}

