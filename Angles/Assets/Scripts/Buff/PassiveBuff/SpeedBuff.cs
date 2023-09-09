using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : PassiveBuff
{
    IAvatar m_avatar;
    float m_speedVariation;

    public SpeedBuff(string name, int maxCount, string effectName, float speedVariation) : base(name, maxCount, effectName)
    {
        m_speedVariation = speedVariation;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        m_avatar.Speed.IntervalValue -= m_speedVariation;
    }

    public override void OnStart(GameObject caster)
    {
        // �÷��̾�, ���� ���� Ŭ���� TryGetComponent �ؿ���
        base.OnStart(caster);

        caster.TryGetComponent(out IAvatar avatar); // Avatar ������ �̷� ������ �޾ƿͼ� �����غ��� --> �������̽� ������Ƽ ���
        if (avatar == null) return;
        
        m_avatar = avatar;
        m_avatar.Speed.IntervalValue += m_speedVariation;
    }
}

public class SpeedTimerBuff : TimeBuff
{
    IAvatar m_avatar;
    float m_speedVariation;

    public SpeedTimerBuff(string name, int maxCount, string effectName, float duration, float maxTickCount, float speedVariation) : base(name, maxCount, effectName, duration, maxTickCount)
    {
        m_speedVariation = speedVariation;
    }

    public override void ApplyTickEffect()
    {
    }

    public override void OnEnd()
    {
        base.OnEnd();
        m_avatar.Speed.IntervalValue -= m_speedVariation;
    }

    public override void OnStart(GameObject caster)
    {
        // �÷��̾�, ���� ���� Ŭ���� TryGetComponent �ؿ���
        base.OnStart(caster);

        caster.TryGetComponent(out IAvatar avatar); // Avatar ������ �̷� ������ �޾ƿͼ� �����غ��� --> �������̽� ������Ƽ ���
        if (avatar == null) return;

        m_avatar = avatar;
        m_avatar.Speed.IntervalValue += m_speedVariation;
    }
}
