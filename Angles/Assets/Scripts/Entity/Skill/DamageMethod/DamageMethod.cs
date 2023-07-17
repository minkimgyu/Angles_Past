using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageSupportData // --> 추후에 버프 추가
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
    protected EffectMethod effectMethod; // 효과들 모음 --> 이팩트는 이름으로 오브젝트 풀링에서 불러옴

    /// <summary>
    /// 
    /// </summary>
    /// <param name="caster">battleComponent를 가지고 있는 오브젝트</param>
    /// <param name="me"> 이 메소드를 컴포넌트로 담고있는 오브젝트</param>
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

//abstract public class TickBaseDamageMethod : DamageMethod // --> 스킬 사용 전 딜레이 + 공격 사이 사이의 딜레이 둘 다 같이 구현 가능할 듯
//{
//    // 토큰 추가 --> 함수 내에서 사용하게끔 변경
//}

// 데미지를 입히는 오브젝트를 생성하는 스킬의 경우 Action으로 데미지 함수 연결해서 적용
// 오브젝트 풀링에서 꺼낼 때 연결
// 다시 집어넣을 때 연결 해제