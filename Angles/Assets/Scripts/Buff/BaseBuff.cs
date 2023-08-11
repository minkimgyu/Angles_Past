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
    public void Init(BuffData data);

    public void OnStart(GameObject caster, BuffEffectComponent effectComponent); // getComponent

    public void OnEnd(BuffEffectComponent effectComponen);

    public void Tick(float deltaTime);

    public IBuff CreateCopy(BuffData data);
}

abstract public class BaseBuff<T> : IBuff  // --> �پ��� �����Ϳ� ���� �����ϰԲ� ���� ---> PlayerData, EnemyData, HealthData
{
    public BaseBuff(BuffData data)
    {
        Init(data);
        isFinished = false;
    }

    protected T variationData; // ���� ������ ���̽����� �����ͼ� �� ������ ������ ������

    [SerializeField]
    protected BuffData m_data;
    public BuffData Data { get { return m_data; } }

    protected bool isFinished;
    public bool IsFinished { get { return isFinished; } }

    //protected BasicEffectPlayer m_effectPlayer;

    //[SerializeField]
    //protected EffectMethod effectMethod;

    public void Init(BuffData data) => m_data = data;

    public abstract void OnStart(GameObject caster, BuffEffectComponent effectComponent); // getComponent

    public abstract void OnEnd(BuffEffectComponent effectComponen);

    public virtual void Tick(float deltaTime) { }

    public abstract IBuff CreateCopy(BuffData data); // �̰Ŵ� ������ ���� Ŭ�������� ����


    public void DoUpdate(float deltaTime)
    {
        Tick(deltaTime);
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

//[CreateAssetMenu(fileName = "PassiveBuff", menuName = "Buff/PassiveBuff", order = int.MaxValue)]
abstract public class PassiveBuff<T> : BaseBuff<T>
{
    public PassiveBuff(BuffData data) : base(data)
    {
    }
}