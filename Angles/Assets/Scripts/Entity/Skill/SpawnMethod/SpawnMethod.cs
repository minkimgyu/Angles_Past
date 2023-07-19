using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnSupportData // --> 추후에 버프 추가
{
    public SpawnSupportData(GameObject caster, SpawnSkill me)
    {
        m_caster = caster;
        m_me = me;
    }

    GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }

    SpawnSkill m_me;
    public SpawnSkill Me { get { return m_me; } }
}

abstract public class SpawnMethod : ScriptableObject
{
    [SerializeField]
    protected EffectMethod effectMethod; // 효과들 모음 --> 이팩트는 이름으로 오브젝트 풀링에서 불러옴

    [SerializeField]
    protected string projectileName;

    public abstract void Execute(SpawnSupportData supportData);
}
