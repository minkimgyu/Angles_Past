using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class DamageMethod : ScriptableObject
{
    public abstract void Attack(GameObject go, SkillData data);

    protected bool DamageToEntity(GameObject me, Transform enemy, SkillData data)
    {
        enemy.TryGetComponent(out IHealth health);

        if (health == null || data.CanHitSkill(health.ReturnTag()) == false) return false;

        health.UnderAttack(data.Damage);
        health.Knockback(me.transform.position - enemy.position, data.KnockBackThrust);

        return true;
    }
}

abstract public class TickBaseDamageMethod : DamageMethod // --> ��ų ��� �� ������ + ���� ���� ������ ������ �� �� ���� ���� ������ ��
{
    // ��ū �߰� --> �Լ� ������ ����ϰԲ� ����
}

// �������� ������ ������Ʈ�� �����ϴ� ��ų�� ��� Action���� ������ �Լ� �����ؼ� ����
// ������Ʈ Ǯ������ ���� �� ����
// �ٽ� ������� �� ���� ����