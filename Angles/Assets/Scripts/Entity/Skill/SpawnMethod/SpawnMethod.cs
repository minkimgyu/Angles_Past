using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnSupportData // --> ���Ŀ� ���� �߰�
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
    protected EffectMethod effectMethod; // ȿ���� ���� --> ����Ʈ�� �̸����� ������Ʈ Ǯ������ �ҷ���

    [SerializeField]
    protected string projectileName;

    public abstract void Execute(SpawnSupportData supportData);
}
