using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BasicProjectile : MonoBehaviour
{
    Transform m_posTr;
    public Transform PosTr { get { return m_posTr; } set { m_posTr = value; } }

    protected BattleComponent m_battleComponent;
    public BattleComponent BattleComponent { get { return m_battleComponent; } }

    protected ContactComponent m_contactComponent; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    [SerializeField]
    GrantedUtilization grantedUtilization;

    protected bool isFinished;
    public bool IsFinished { get { return isFinished; } }

    public abstract void DoUpdate();

    protected virtual void Awake()
    {
        m_battleComponent = GetComponent<BattleComponent>();
        m_contactComponent = GetComponent<ContactComponent>();
    }
    private void Start()
    {
        grantedUtilization.LootSkillFromDB(BattleComponent);
    }

    public virtual void Init(Transform tr)
    {
        m_posTr = tr;
    }

    public virtual void Init(Vector3 pos)
    {
        transform.localPosition = pos;
        m_posTr = null;
    }

    public virtual void OnEnd()
    {
        isFinished = false;
        gameObject.SetActive(false);
    }

    protected virtual void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        ContactComponent.CallWhenCollisionEnter(col);

        m_battleComponent.UseSkill(SkillUseConditionType.Contact); // --> 이런 식으로 스킬로 작동
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        ContactComponent.CallWhenCollisionExit(col);
    }

    protected virtual void OnDisable()
    {
        m_posTr = null;
        ObjectPooler.ReturnToPool(gameObject);
    }
}