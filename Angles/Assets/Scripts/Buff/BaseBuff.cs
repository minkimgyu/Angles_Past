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
    string buffName;
    public string BuffName { get { return buffName; } }

    protected bool isFinished;
    public bool IsFinished { get { return isFinished; } }

    public abstract void OnStart(GameObject caster); // getComponent

    public abstract void OnEnd();

    public abstract void Tick(float deltaTime);

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}

//[CreateAssetMenu(fileName = "TimeBuff", menuName = "Buff/TimeBuff", order = int.MaxValue)]
abstract public class TimeBuff : BaseBuff
{
    float maxTickTime;

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