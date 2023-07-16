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

abstract public class TickBaseDamageMethod : DamageMethod // --> 스킬 사용 전 딜레이 + 공격 사이 사이의 딜레이 둘 다 같이 구현 가능할 듯
{
    // 토큰 추가 --> 함수 내에서 사용하게끔 변경
}

// 데미지를 입히는 오브젝트를 생성하는 스킬의 경우 Action으로 데미지 함수 연결해서 적용
// 오브젝트 풀링에서 꺼낼 때 연결
// 다시 집어넣을 때 연결 해제