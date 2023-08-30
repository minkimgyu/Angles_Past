using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseBuff  // --> �پ��� �����Ϳ� ���� �����ϰԲ� ���� ---> PlayerData, EnemyData, HealthData
{
    public BaseBuff(string name, int maxCount)
    {
        this.name = name;
        this.maxCount = maxCount;
    }

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
    // 

<<<<<<< Updated upstream
    public abstract void OnStart(GameObject caster); // ���� ����Ʈ�� �̹�Ʈ�� AddState --> BuffComponent���� SO �޾Ƽ� ó��
=======
    public abstract void OnStart(GameObject caster); // getComponent
>>>>>>> Stashed changes

    public abstract void OnEnd();

    public virtual void Tick(float deltaTime) { }

    //public abstract IBuff CreateCopy(BuffData data); // �̰Ŵ� ������ ���� Ŭ�������� ����

    public void DoUpdate(float deltaTime)
    {
        Tick(deltaTime);
    }
}

abstract public class TimeBuff : BaseBuff
{
    public TimeBuff(string name, int maxCount, float duration, float maxTickTime) : base(name, maxCount)
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

public class TimeBuff1 : TimeBuff 
{
    public TimeBuff1(string name, int maxCount, float duration, float maxTickTime) : base(name, maxCount, duration, maxTickTime)
    {
    }

    public override void ApplyTickEffect()
    {
        throw new NotImplementedException();
    }

    public override void OnEnd()
    {
        throw new NotImplementedException();
    }

    public override void OnStart(GameObject caster)
    {
        throw new NotImplementedException();
    }
}

public class PassiveBuff1 : PassiveBuff
{
    public PassiveBuff1(string name, int maxCount) : base(name, maxCount)
    {
    }

    public override void OnEnd()
    {
        throw new NotImplementedException();
    }

    public override void OnStart(GameObject caster)
    {
        throw new NotImplementedException();
    }
}

abstract public class PassiveBuff : BaseBuff
{
    public PassiveBuff(string name, int maxCount) : base(name, maxCount)
    {
    }
}