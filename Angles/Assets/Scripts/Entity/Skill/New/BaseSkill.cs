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
//        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // ����Ʈ �̸� �߰�
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
//        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // ����Ʈ �̸� �߰�
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

    // --> ��ų ��� ������ ���� �����ִ� Player�� ���������ش�.

    public BaseMethod(Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas)
    {
        m_effectDatas = effectDatas;
        m_soundDatas = soundDatas;
    }

    //public abstract void AddState(SkillData data); // ������ ���̽����� Stat�� ī���ؼ� ������

    public virtual void Execute(SkillSupportData supportData, T target) { }

    public virtual void Execute(SkillSupportData supportData) { }

    protected void PlaySound(Transform target, SoundData data)
    {
        SoundManager.instance.PlaySFX(target.position, data.name, data.volume);
    }

    protected bool PlayEffect(Transform target, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // ����Ʈ �̸� �߰�
        if (player == null) return false;

        player.Init(target, data.disableTime);
        player.PlayEffect();

        m_effectplayers.Add(player);
        return true;
    }

    protected bool PlayEffect(Vector3 targetPos, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // ����Ʈ �̸� �߰�
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
    /// ��ų�� ������ �� ����
    /// </summary>
    public enum OverlapType { None, Restart, CountUp } // �� �ٸ� ��ų�� ����Ʈ�� �־ ���, ������ �����ϴ� ��ų�� �ٽ� �����ؼ� ���

    /// <summary>
    /// ��ų ��� ����, InRange, OutRange�� ���� ���� �� �÷��̾ ���� ������ ���Դ��� �ƴ��� üũ��
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

    protected string m_preDelayEffectName; // null�̸� ����Ʈ ��� X

    protected int m_useCountDownPoint; // 0 �Ǵ� 1�� ���� ����

    protected int m_useCountUpPoint; // 0 �Ǵ� 1�� ���� ����

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
    public abstract void Execute(); // ��ų ����
    public abstract void End(); // ���� �� ���� ����
    public abstract void Reset(); // ��ų �� ���� ���
}


public class Skill<T> : BaseSkill // ���� ���� ����
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

    public override void Init(GameObject caster) // fix ���� �־��ֱ�
    {
        m_caster = caster;
        m_specifyLocation.Init(caster); // IsFix�� data ������ ����
    }

    public override void Execute()
    {
        m_isRunning = true;

        float tickDuration = m_duration / m_tickCount;
        if (m_storedPreDelay >= m_preDelay)
        {
            // Predelay Effect �����غ���
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
        // ���� ������
        Reset();
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