using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T는 데이터 클레스
/// </summary>
/// <typeparam name="T"></typeparam>

//interface IBuff
//{
//    public void OnStart();

//    public void OnUpdate();

//    public void OnEnd();
//}

abstract public class BaseBuff : ScriptableObject
{
    protected bool canOverlap;

    public bool CanOverlap
    {
        get { return canOverlap; }
    }

    public abstract void OnStart();

    public abstract void OnUpdate();

    public abstract void OnEnd();
}

//[CreateAssetMenu(fileName = "TimeBuff", menuName = "Buff/TimeBuff", order = int.MaxValue)]
abstract public class TimeBuff : BaseBuff
{
    BuffComponent m_bC;

    public TimeBuff(BuffComponent bC)
    {
        m_bC = bC;
    }

    float buffTime;

    public bool CheckLifeTime()
    {
        buffTime -= Time.deltaTime;

        if (buffTime <= 0) return false;
        else return true;
    }

    public override void OnUpdate()
    {
        if (CheckLifeTime() == false) m_bC.RemoveBuff(this);
    }
}

//[CreateAssetMenu(fileName = "PassiveBuff", menuName = "Buff/PassiveBuff", order = int.MaxValue)]
abstract public class PassiveBuff : BaseBuff
{
   
}