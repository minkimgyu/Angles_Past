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
        // �÷��̾�, ���� ���� Ŭ���� TryGetComponent �ؿ���

        caster.TryGetComponent(out IAvatar avatar); // Avatar ������ �̷� ������ �޾ƿͼ� �����غ��� --> �������̽� ������Ƽ ���
        if (avatar == null) return;
        
        m_avatar = avatar;
        m_avatar.Speed.IntervalValue += m_speedVariation;
    }
}
