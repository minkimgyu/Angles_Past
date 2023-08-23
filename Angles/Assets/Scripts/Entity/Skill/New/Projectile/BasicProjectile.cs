using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BasicProjectile : MonoBehaviour
{
    protected Transform m_caster;
    public Transform Caster { get { return m_caster; } set { m_caster = value; } }

    protected SkillController m_skillController;
    public SkillController SkillController { get { return m_skillController; } }

    protected ContactComponent m_contactComponent; // --> ������, ���� ��� ����� ������ �־�����
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    [SerializeField]
    GrantedSkill grantedUtilization;

    protected bool isFinished;
    public bool IsFinished { get { return isFinished; } }

    public abstract void DoUpdate();

    protected virtual void Awake()
    {
        m_skillController = GetComponent<SkillController>();
        m_contactComponent = GetComponent<ContactComponent>();
    }
    private void Start()
    {
    }

    public virtual void Init(Transform tr)
    {
        m_caster = tr;
        ResetAndUseSkill();
    }

    public virtual void Shoot(Vector2 dir, float thrust) { } // �̰ɷ� �߻� ����

    public virtual void Init(Vector3 pos)
    {
        transform.position = pos;
        m_caster = null;
        ResetAndUseSkill();
    }

    void ResetAndUseSkill()
    {
        grantedUtilization.LootSkillFromDB(m_skillController);
        m_skillController.UseSkill(BaseSkill.UseConditionType.Init); // --> �̷� ������ ��ų�� �۵�
    }

    protected void NowFinish()
    {
        isFinished = true;
    }

    public virtual void OnEnd()
    {
        isFinished = false;
        gameObject.SetActive(false);
    }

    protected virtual void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ
    {
        ContactComponent.CallWhenCollisionEnter(col);

        m_skillController.UseSkill(BaseSkill.UseConditionType.Contact); // --> �̷� ������ ��ų�� �۵�
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        ContactComponent.CallWhenCollisionExit(col);
    }

    protected virtual void OnDisable()
    {
        m_caster = null;
        ObjectPooler.ReturnToPool(gameObject);
    }
}