using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public bool IsTarget(EntityTag tag);

    public void UnderAttack(float healthPoint);

    public void Heal(float healthPoint);

    public void Die();
}

//abstract public class HealthEntity<T, W> : StateMachineEntity<T, W>
//{
//    protected float maxHp;
//    protected float minHp;
//    protected float hp;

//    protected bool death;

//    public float Hp { get { return hp; } set { hp = value; } }
//    public float MinHp { get { return minHp; } set { minHp = value; } }
//    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
//    public bool Death { get { return death; } set { death = value; } }

//    public virtual void UnderAttack(float healthPoint, float knockBackThrust)
//    {
//        if(hp > minHp)
//        {
//            hp -= healthPoint;
//            if(hp <= minHp)
//            {
//                Die(true);
//                hp = 0;
//            }

//            KnockBack(knockBackThrust);
//        }
//    }

//    public abstract void KnockBack(float knockBackThrust);

//    public virtual void Heal(float healthPoint)
//    {
//        if (maxHp > hp)
//        {
//            hp += healthPoint;
//            if (hp >= maxHp)
//            {
//                hp = maxHp;
//            }
//        }
//    }

//    public virtual void Die(bool nowDie)
//    {
//        death = nowDie;
//    }
//}