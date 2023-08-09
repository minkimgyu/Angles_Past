using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SkillSupportData // --> 추후에 버프 추가
{
    public SkillSupportData(GameObject caster, SkillData data, Vector3 pos, int tickCount)//, int tickCount)
    {
        m_caster = caster;
        m_data = data;
        m_Pos = pos;
        m_tickCount = tickCount;
    }

    GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }

    SkillData m_data;
    public SkillData Data { get { return m_data; } }

    Vector3 m_Pos;
    public Vector3 Pos { get { return m_Pos; } }

    int m_tickCount;
    public int TickCount { get { return m_tickCount; } }
}

public struct DamageSupportData
{
    public string name; // --> 데이터 베이스에서 스킬 이름을 바탕(현 prefabData)으로 해당 데이터를 찾아서 넣어줌
    public Dictionary<EffectName, EffectData> effectDatas;

    //public List<float> attackCircleRange; // 틱마다 범위가 달라짐

}

public struct BuffSupportData
{
    public string name; // --> 데이터 베이스에서 스킬 이름을 바탕(현 prefabData)으로 해당 데이터를 찾아서 넣어줌
    public Dictionary<EffectName, EffectData> effectDatas;
    public List<string> buffNames;

    //public List<float> attackCircleRange; // 틱마다 범위가 달라짐

}

public enum EffectName
{
    PunchEffect,
    AttackSkillEffect,

    HitSurfaceEffect,

    BuffEffect,

    SpawnEffect
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

    public EffectSource() // 사운드 플레이 이밴트 트리거, 이팩트 데이터
    {

    }

    protected bool PlayEffect(Transform target, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = true;
        player.Init(target, effectData.duration);
        player.PlayEffect();

        SoundManager.instance.PlaySFX(target.position, data.soundName, data.volume);

        effectPlayer = player;
        return true;
    }

    protected bool PlayEffect(Vector3 pos, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = false;
        player.Init(pos, effectData.duration);
        player.PlayEffect();

        effectPlayer = player;
        return true;
    }

    protected void StopEffect()
    {
        effectPlayer.StopEffect();
        effectPlayer = null;
    }
}

public struct SpawnSupportData
{
    public string name; // --> 데이터 베이스에서 스킬 이름을 바탕(현 prefabData)으로 해당 데이터를 찾아서 넣어줌

    public Dictionary<EffectName, EffectData> effectDatas;
    public float speed;
    public float distanceFromCaster;

    public string projectileName;
    public int projectileCount;
}

abstract public class BaseMethod<T>
{
    List<BasicEffectPlayer> effectPlayers;

    public abstract void Init(SkillData data); // 데이터 베이스에서 Stat을 카피해서 가져옴

    public virtual void Execute(SkillSupportData supportData, T target) { }

    public virtual void Execute(SkillSupportData supportData) { }

    protected bool PlayEffect(Transform target, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = true;
        player.Init(target, data.duration);
        player.PlayEffect();

        effectPlayers.Add(player);
        return true;
    }

    protected bool PlayEffect(Vector3 pos, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = false;
        player.Init(pos, data.duration);
        player.PlayEffect();

        effectPlayers.Add(player);
        return true;
    }

    protected void StopEffect()
    {
        for (int i = 0; i < effectPlayers.Count; i++)
        {
            effectPlayers[i].StopEffect();
            effectPlayers[i] = null;
        }
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
        return new BaseSkill<T>(m_specifyLocation, m_targetDesignation, m_baseMethods);
    }

    [SerializeField]
    protected SkillData m_data; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    public SkillData Data { get { return m_data; } }

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

    public BaseSkill(SpecifyLocation specifyLocation, TargetDesignation<T> targetDesignation, List<BaseMethod<T>> methods)
    {
        m_specifyLocation = specifyLocation;
        m_targetDesignation = targetDesignation;
        m_baseMethods = methods;
    }

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

        SkillSupportData supportData = new SkillSupportData(m_specifyLocation.Caster, m_data, myPos, tickCount);

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
    public CasterCircleRangeAttack(SpecifyLocation specifyLocation, TargetDesignation<RaycastHit2D[]> targetDesignation, List<BaseMethod<RaycastHit2D[]>> methods) : base(specifyLocation, targetDesignation, methods)
    {
    }
}

