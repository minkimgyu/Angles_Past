using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SkillSupportData // --> ���Ŀ� ���� �߰�
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
    public string name; // --> ������ ���̽����� ��ų �̸��� ����(�� prefabData)���� �ش� �����͸� ã�Ƽ� �־���
    public Dictionary<EffectName, EffectData> effectDatas;

    //public List<float> attackCircleRange; // ƽ���� ������ �޶���

}

public struct BuffSupportData
{
    public string name; // --> ������ ���̽����� ��ų �̸��� ����(�� prefabData)���� �ش� �����͸� ã�Ƽ� �־���
    public Dictionary<EffectName, EffectData> effectDatas;
    public List<string> buffNames;

    //public List<float> attackCircleRange; // ƽ���� ������ �޶���

}

public enum EffectName
{
    PunchEffect,
    AttackSkillEffect,

    HitSurfaceEffect,

    BuffEffect,

    SpawnEffect
}

public struct EffectData // ����Ʈ �ð�, ũ�� ��� ���� �����ϰԲ� ����
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

    public EffectSource() // ���� �÷��� �̹�Ʈ Ʈ����, ����Ʈ ������
    {

    }

    protected bool PlayEffect(Transform target, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // ����Ʈ �̸� �߰�
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
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectData.name); // ����Ʈ �̸� �߰�
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
    public string name; // --> ������ ���̽����� ��ų �̸��� ����(�� prefabData)���� �ش� �����͸� ã�Ƽ� �־���

    public Dictionary<EffectName, EffectData> effectDatas;
    public float speed;
    public float distanceFromCaster;

    public string projectileName;
    public int projectileCount;
}

abstract public class BaseMethod<T>
{
    List<BasicEffectPlayer> effectPlayers;

    public abstract void Init(SkillData data); // ������ ���̽����� Stat�� ī���ؼ� ������

    public virtual void Execute(SkillSupportData supportData, T target) { }

    public virtual void Execute(SkillSupportData supportData) { }

    protected bool PlayEffect(Transform target, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // ����Ʈ �̸� �߰�
        if (player == null) return false;

        player.IsFixed = true;
        player.Init(target, data.duration);
        player.PlayEffect();

        effectPlayers.Add(player);
        return true;
    }

    protected bool PlayEffect(Vector3 pos, EffectData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.name); // ����Ʈ �̸� �߰�
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
    public void Execute(); // ��ų ����

    public void Finish(); // ��ų ��� ����

    public void End(); // ���� �� ���� ����
    public void Reset(); // ��ų �� ���� ���

    public ISkill CreateCopy();
}

public class BaseSkill<T> : ISkill
{
    public ISkill CreateCopy()
    {
        return new BaseSkill<T>(m_specifyLocation, m_targetDesignation, m_baseMethods);
    }

    [SerializeField]
    protected SkillData m_data; // --> ������, ���� ��� ����� ������ �־�����
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
        m_specifyLocation.Init(caster, true); // IsFix�� data ������ ����

        for (int i = 0; i < m_baseMethods.Count; i++) // �����;��� ������ ���� ���� ���, ���⼭ �ε����� �Ѱܼ� ȣ���Ű��
        {
            m_baseMethods[i].Init(data); // �����͸� ���� SO�� ȣ���ؼ� �ʱ�ȭ������
        }
    }

    public virtual void Execute() // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
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
        // ���� ������
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

