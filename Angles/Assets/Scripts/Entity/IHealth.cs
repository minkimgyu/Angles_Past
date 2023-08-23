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

// 투사체 체력을 만들고 싶다면 아래 인터페이스 조금 고쳐서 상속시키기
// Speed StunTime Weight Mass Drag DieEffectName 없앤 새로운 인터페이스 넣기
public interface IHealth : ITag // 프로퍼티로 변수를 추가해보자 --> healthyEntityData
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

    public BuffFloat Weight { get; set; } // 실질적 Entity 무게

    public BuffFloat Mass { get; set; } // rigidbody mass

    public BuffFloat Drag { get; set; }
}

/// <summary>
/// 움직일 수 있고, 체력이 있고, 버프를 걸 수 있음
/// </summary>
public interface IAvatar : IHealth, IMoveable, IBuffInvoker
{
}