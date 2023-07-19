using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BasicProjectile : MonoBehaviour
{
    Transform m_posTr;
    public Transform PosTr { get { return m_posTr; } set { m_posTr = value; } }

    protected SpawnSkill m_skill;
    public SpawnSkill Skill { get { return m_skill; } }

    protected BattleComponent m_battleComponent;
    public BattleComponent BattleComponent { get { return m_battleComponent; } }

    protected ContactComponent m_contactComponent; // --> ������, ���� ��� ����� ������ �־�����
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    [SerializeField]
    string skillName;

    private void Awake()
    {
        m_battleComponent = GetComponent<BattleComponent>();
        m_contactComponent = GetComponent<ContactComponent>();
    }
    private void Start()
    {
        m_battleComponent.LootingSkill(DatabaseManager.Instance.UtilizationDB.SkillCallDatas.Find(x => x.Name == skillName).CopyData());
    }

    public virtual void Init(Transform tr, SpawnSkill skill)
    {
        m_posTr = tr;
        m_skill = skill;
    }

    public virtual void Init(Vector3 pos, SpawnSkill skill)
    {
        transform.localPosition = pos;
        m_posTr = null;
        m_skill = skill;
    }

    protected virtual void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ
    {
        ContactComponent.CallWhenCollisionEnter(col);

        m_battleComponent.UseSkill(SkillUseConditionType.Contact); // --> �̷� ������ ��ų�� �۵�
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        ContactComponent.CallWhenCollisionExit(col);
    }

    protected virtual void OnDisable()
    {
        m_skill = null;
        ObjectPooler.ReturnToPool(gameObject);
    }
}