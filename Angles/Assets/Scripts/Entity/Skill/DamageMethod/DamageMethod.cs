using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageSupportData // --> ���Ŀ� ���� �߰�
{
    public DamageSupportData(GameObject caster, AttackSkill me, int tickCount)
    {
        m_caster = caster;
        m_me = me;
        m_tickCount = tickCount;
    }

    GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }

    AttackSkill m_me;
    public AttackSkill Me { get { return m_me; } }

    int m_tickCount;
    public int m_TickCount { get { return m_tickCount; } }
}

abstract public class DamageMethod : ScriptableObject
{
    [SerializeField]
    protected EffectMethod effectMethod; // ȿ���� ���� --> ����Ʈ�� �̸����� ������Ʈ Ǯ������ �ҷ���

    /// <summary>
    /// 
    /// </summary>
    /// <param name="caster">battleComponent�� ������ �ִ� ������Ʈ</param>
    /// <param name="me"> �� �޼ҵ带 ������Ʈ�� ����ִ� ������Ʈ</param>
    /// <param name="data"></param>
    /// <param name="effectMethod"></param>
    public abstract void Execute(DamageSupportData supportData);

    protected bool DamageToEntity(GameObject me, Transform enemy, SkillData data)
    {
        enemy.TryGetComponent(out IHealth health);

        if (health == null || data.CanHitSkill(health.ReturnTag()) == false) return false;

        health.UnderAttack(data.Damage, (me.transform.position - enemy.position).normalized, data.KnockBackThrust);
        return true;
    }
}

//abstract public class TickBaseDamageMethod : DamageMethod // --> ��ų ��� �� ������ + ���� ���� ������ ������ �� �� ���� ���� ������ ��
//{
//    // ��ū �߰� --> �Լ� ������ ����ϰԲ� ����
//}

// �������� ������ ������Ʈ�� �����ϴ� ��ų�� ��� Action���� ������ �Լ� �����ؼ� ����
// ������Ʈ Ǯ������ ���� �� ����
// �ٽ� ������� �� ���� ����