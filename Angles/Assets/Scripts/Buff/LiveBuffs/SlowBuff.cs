using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeBuff", menuName = "Buff/TimeBuff", order = int.MaxValue)]
public class SlowBuff : PassiveBuff
{
    HealthEntityData m_data;

    [SerializeField]
    float ratio;

    public SlowBuff(HealthEntityData data)
    {
        m_data = data;
    }

    public override void OnEnd()
    {
        m_data.Speed += ratio;
    }

    public override void OnStart()
    {
        m_data.Speed -= ratio;
    }

    public override void OnUpdate()
    {
       
    }
}
