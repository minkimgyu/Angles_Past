using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageSupportData // --> ���Ŀ� ���� �߰�
{
    public DamageSupportData(GameObject caster, GameObject me, SkillData data)
    {
        m_caster = caster;
        m_me = me;
        m_data = data;
    }

    GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }

    GameObject m_me;
    public GameObject Me { get { return m_me; } }

    SkillData m_data;
    public SkillData Data { get { return m_data; } }
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
    public abstract void Attack(DamageSupportData supportData);

    protected bool DamageToEntity(GameObject me, Transform enemy, SkillData data)
    {
        enemy.TryGetComponent(out IHealth health);

        if (health == null || data.CanHitSkill(health.ReturnTag()) == false) return false;

        health.UnderAttack(data.Damage);
        health.Knockback((me.transform.position - enemy.position).normalized, data.KnockBackThrust);

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