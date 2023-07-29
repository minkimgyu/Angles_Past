using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBuff<T>
{
    public T GetData();
}

abstract public class BaseBuff : MonoBehaviour
{
    [SerializeField]
    BuffData data;
    public BuffData Data { get { return data; } }

    [SerializeField]
    int buffCount; // buffData 제작해서 이름, 최대 갯수 넣기
    public int BuffCount { get { return buffCount; } }

    protected bool isFinished;
    public bool IsFinished { get { return isFinished; } }

    //protected BasicEffectPlayer m_effectPlayer;

    //[SerializeField]
    //protected EffectMethod effectMethod;

    public void Init(BuffData data)
    {
        this.data = data;
    }

    public abstract void OnStart(GameObject caster, BuffEffectComponent effectComponent); // getComponent

    public abstract void OnEnd(BuffEffectComponent effectComponen);

    public abstract void Tick(float deltaTime);

    private void OnDisable()
    {
        data = null;
        ObjectPooler.ReturnToPool(gameObject);
    }
}

//[CreateAssetMenu(fileName = "TimeBuff", menuName = "Buff/TimeBuff", order = int.MaxValue)]
abstract public class TimeBuff : BaseBuff
{
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
abstract public class PassiveBuff : BaseBuff
{
    public override void Tick(float deltaTime)
    {
       
    }
}