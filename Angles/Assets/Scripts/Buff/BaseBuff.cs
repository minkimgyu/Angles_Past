using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public interface IEntityData<T>
//{
//    public T ReturnEntityData();
//}

public interface IBuff
{
    public bool IsFinished { get; set; }

    public void Init(BuffData data);

    public void OnStart(GameObject caster); // getComponent

    public void OnEnd();

    public void Tick(float deltaTime);

    public BuffData ReturnBuffData();

    public IBuff CreateCopy(BuffData data);
}

abstract public class BaseBuff<T> : IBuff  // --> 다양한 데이터에 접근 가능하게끔 제작 ---> PlayerData, EnemyData, HealthData
{
    public BaseBuff(BuffData data)
    {
        Init(data);
        IsFinished = false;
    }

    protected T variationData; // 버프 데이터 베이스에서 가져와서 이 값으로 증감을 적용함

    [SerializeField]
    protected BuffData m_data;
    public BuffData Data { get { return m_data; } }

    public bool IsFinished { get; set; }

    //protected BasicEffectPlayer m_effectPlayer;

    //[SerializeField]
    //protected EffectMethod effectMethod;

    public void Init(BuffData data) => m_data = data;

    public abstract void OnStart(GameObject caster); // 버프 이팩트는 이밴트로 Init --> BuffComponent에서 SO 받아서 처리

    public abstract void OnEnd();

    public virtual void Tick(float deltaTime) { }

    public abstract IBuff CreateCopy(BuffData data); // 이거는 각각의 하위 클레스에서 제작


    public void DoUpdate(float deltaTime)
    {
        Tick(deltaTime);
    }

    public BuffData ReturnBuffData()
    {
        return m_data;
    }

    //private void OnDisable()
    //{
    //    data = null;
    //    ObjectPooler.ReturnToPool(gameObject);
    //}
}

//[CreateAssetMenu(fileName = "TimeBuff", menuName = "Buff/TimeBuff", order = int.MaxValue)]
abstract public class TimeBuff<T> : BaseBuff<T>
{
    public TimeBuff(BuffData data) : base(data)
    {
    }

    [SerializeField]
    float maxTickTime;

    [SerializeField]
    float buffTime;
    float tickTime;

    public override void Tick(float deltaTime)
    {
        buffTime -= deltaTime;

        if (buffTime <= 0)
        {
            IsFinished = true;
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

//[CreateAssetMenu(fileName = "PassiveBuff", menuName = "Buff/PassiveBuff", order = int.MaxValue)]
abstract public class PassiveBuff<T> : BaseBuff<T>
{
    public PassiveBuff(BuffData data) : base(data)
    {
    }
}