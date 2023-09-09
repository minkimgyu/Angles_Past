using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
abstract public class BaseBuff  // --> �پ��� �����Ϳ� ���� �����ϰԲ� ���� ---> PlayerData, EnemyData, HealthData
{
    public BaseBuff(string name, int maxCount, string effectName)
    {
        this.name = name;
        this.maxCount = maxCount;
        this.effectName = effectName;
    }

    protected string effectName;

    // ���� ����
    protected string name;
    public string Name
    {
        get { return name; }
    }

    protected int maxCount;
    public int MaxCount
    {
        get { return maxCount; }
    }

    protected bool isFinished;
    public bool IsFinished
    {
        get { return isFinished; }
    }

    BasicEffectPlayer basicEffectPlayer;

    public virtual void OnStart(GameObject caster) // ���� ����Ʈ�� �̹�Ʈ�� AddState --> BuffComponent���� SO �޾Ƽ� ó��
    {
        basicEffectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(effectName);

        basicEffectPlayer.transform.position = caster.transform.position;
        basicEffectPlayer.Init(caster.transform);

        basicEffectPlayer.PlayEffect();
    }

    public virtual void OnEnd()
    {
        basicEffectPlayer.StopEffect();
        basicEffectPlayer = null;
    }

    public virtual void Tick(float deltaTime) { }

    //public abstract IBuff CreateCopy(BuffData data); // �̰Ŵ� ������ ���� Ŭ�������� ����

    public void DoUpdate(float deltaTime)
    {
        Tick(deltaTime);
    }
}

abstract public class TimeBuff : BaseBuff
{
    public TimeBuff(string name, int maxCount, string effectName, float duration, float maxTickTime) : base(name, maxCount, effectName)
    {
        this.duration = duration;
        this.maxTickTime = maxTickTime;
    }

    public float duration; // ���� ���� �ð�
    public float maxTickTime; // ƽ ������ �ð�
    
    float tickTime;

    public override void Tick(float deltaTime)
    {
        duration -= deltaTime;

        if (duration <= 0)
        {
            isFinished = true;
        }

        tickTime -= deltaTime;
        if (tickTime <= 0)
        {
            ApplyTickEffect();
            tickTime = maxTickTime;
        }
    }

    public abstract void ApplyTickEffect();
}

abstract public class PassiveBuff : BaseBuff
{
    public PassiveBuff(string name, int maxCount, string effectName) : base(name, maxCount, effectName)
    {
    }
}