using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : PassiveBuff
{
    IAvatar m_avatar;
    float m_speedVariation;

    public SpeedBuff(string name, int maxCount, float speedVariation) : base(name, maxCount)
    {
        m_speedVariation = speedVariation;
    }

    public override void OnEnd()
    {
        m_avatar.Speed.IntervalValue -= m_speedVariation;
    }

    public override void OnStart(GameObject caster)
    {
        // 플레이어, 적은 각자 클레스 TryGetComponent 해오기

        caster.TryGetComponent(out IAvatar avatar); // Avatar 변수는 이런 식으로 받아와서 진행해보자 --> 인터페이스 프로퍼티 사용
        if (avatar == null) return;
        
        m_avatar = avatar;
        m_avatar.Speed.IntervalValue += m_speedVariation;
    }
}
