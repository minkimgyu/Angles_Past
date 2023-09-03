using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 움직일 수 있고, 체력이 있고, 버프를 걸 수 있고, 스킬을 사용할 수 있음
/// </summary>
public class Avatar<T> : StateMachineEntity<T>, IAvatar//, IEntityData<W>/*, IBuff<W>*/
{
    public bool Immortality { get; set; }
    public BuffFloat Hp { get; set; }
    public BuffFloat Speed { get; set; }
    public BuffFloat StunTime { get; set; }
    public BuffFloat Weight { get; set; }
    public BuffFloat Mass { get; set; }
    public BuffFloat Drag { get; set; }
    public string DieEffectName { get; set; }

    // 이런 식으로 Entity를 만들어서 리턴해주는 SO 제작
    //public Entity CreateEntity()
    //{
    //    return new PlayerTransform();
    //}

    MoveComponent m_moveComponent;
    public MoveComponent MoveComponent { get { return m_moveComponent; } }

    DashComponent m_dashComponent;
    public DashComponent DashComponent { get { return m_dashComponent; } }

    SkillController m_skillController;
    public SkillController SkillController { get { return m_skillController; } }

    protected GrantedSkill grantedSkill;
    public GrantedSkill GrantedSkill { get { return grantedSkill; } }

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime, 
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames)
    {
        Immortality = immortality;
        Hp = hp.CopyData();
        Speed = speed.CopyData();
        StunTime = stunTime.CopyData();
        Weight = weight.CopyData();
        Mass = mass.CopyData();
        Drag = drag.CopyData();
        DieEffectName = dieEffectName;

        grantedSkill = new GrantedSkill(skillNames);
        grantedSkill.LootSkillFromDB(m_skillController);
    }

    protected BuffController m_buffComponent;
    public BuffController BuffComponent { get { return m_buffComponent; } }

    protected Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody { get { return m_rigidbody; } }

    protected bool m_canHit;
    public bool CanHit { get { return m_canHit; } }

    protected virtual void ShowDieEffect()
    {
        BasicEffectPlayer effectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(DieEffectName);
        if (effectPlayer == null) return;

        effectPlayer.Init(transform.position, 2f);
        effectPlayer.PlayEffect();
    }

    protected virtual void Awake()
    {
        m_buffComponent = GetComponent<BuffController>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_moveComponent = GetComponent<MoveComponent>();
        m_dashComponent = GetComponent<DashComponent>();
        m_skillController = GetComponent<SkillController>();
    }    

    public virtual void Heal(float healthPoint) { }

    public virtual void UnderAttack(float damage, Vector2 dir, float thrust)
    {
        if (m_canHit) return;

        if (Hp.IntervalValue > 0)
        {
            WhenUnderAttack(damage, dir, thrust);
            Hp.IntervalValue -= damage;
            if (Hp.IntervalValue <= 0)
            {
                Die();
                Hp.IntervalValue = 0;
            }
        }
    }

    public virtual void WhenUnderAttack(float damage, Vector2 dir, float thrust)
    {
        m_dicState[CurrentStateName].ReceiveUnderAttack(damage, dir, thrust);
    }

    public virtual void WhenHeal() { }

    public virtual void Die()
    {
        ShowDieEffect();
        gameObject.SetActive(false);
    }

    public EntityTag ReturnEntityTag()
    {
        // 게임 오브젝트 Tag를 EntityTag 이걸로 변환해서 리턴
        return InheritedTag;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SkillController.RemoveAllSkillInList(); // 리스트에 있는 모든 스킬 제거

        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void AddBuffToController(string name)
    {
        m_buffComponent.AddBuff(name);
    }

    public void RemoveBuffToController(string name)
    {
        m_buffComponent.RemoveBuff(name);
    }

    public void AddEnemyContactKnockback() // 플레이어도 사용가능하게끔 변경해보자
    {
        SkillController.AddSkillToList("ContactKnockback");
    }

    public void RemoveEnemyContactKnockback()
    {
        SkillController.RemoveSkillInList("ContactKnockback"); // 만약 안 쓰고 존재한다면 삭제해준다.
    }
}

//BuffController m_buffComponent;
//public BuffController BuffComponent { get { return m_buffComponent; } }

//Rigidbody2D m_rigidbody;
//public Rigidbody2D Rigidbody { get { return m_rigidbody; } }

//bool m_canHit = false;
//public bool CanHit { get { return m_canHit; } set { m_canHit = value; } }

//HealthEntityData m_healthData;

//public HealthEntityData HealthData
//{
//    get
//    {
//        return m_healthData;
//    }
//    set
//    {
//        m_healthData = value;
//        if (m_rigidbody == null) return;

//        m_rigidbody.mass = m_healthData.Mass.IntervalValue;
//        m_rigidbody.drag = m_healthData.Drag.IntervalValue;
//    }
//}
