using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
abstract public class BaseBuff  // --> 다양한 데이터에 접근 가능하게끔 제작 ---> PlayerData, EnemyData, HealthData
{
    public BaseBuff(string name, int maxCount, string effectName)
    {
        this.name = name;
        this.maxCount = maxCount;
        this.effectName = effectName;
    }

    protected string effectName;

    // 변수 모음
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

    public virtual void OnStart(GameObject caster) // 버프 이팩트는 이밴트로 AddState --> BuffComponent에서 SO 받아서 처리
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

    //public abstract IBuff CreateCopy(BuffData data); // 이거는 각각의 하위 클레스에서 제작

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

    public float duration; // 버프 적용 시간
    public float maxTickTime; // 틱 사이의 시간
    
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