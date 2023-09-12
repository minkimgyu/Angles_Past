using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITag
{
    public EntityTag ReturnEntityTag();
}

public interface IBuffInvoker
{
    public void AddBuffToController(string name);

    public void RemoveBuffToController(string name);
}

// ����ü ü���� ����� �ʹٸ� �Ʒ� �������̽� ���� ���ļ� ��ӽ�Ű��
// Speed StunTime Weight Mass Drag DieEffectName ���� ���ο� �������̽� �ֱ�
public interface IHealth : ITag // ������Ƽ�� ������ �߰��غ��� --> healthyEntityData
{
    public bool Immortality { get; set; }

    public BuffFloat Hp { get; set; }

    public string DieEffectName { get; set; }

    public void UnderAttack(float healthPoint, Vector2 dir, float thrust);

    public void Heal(float healthPoint);

    public void Die();

    public void WhenUnderAttack(float healthPoint, Vector2 dir, float thrust);

    public void WhenHeal();
}

public interface IMoveable
{
    public BuffFloat Speed { get; set; }

    public BuffFloat StunTime { get; set; }

    public BuffFloat Weight { get; set; } // ������ Entity ����

    public BuffFloat Mass { get; set; } // rigidbody mass

    public BuffFloat Drag { get; set; }
}

/// <summary>
/// ������ �� �ְ�, ü���� �ְ�, ������ �� �� ����
/// </summary>
public interface IAvatar : IHealth, IMoveable, IBuffInvoker
{
}