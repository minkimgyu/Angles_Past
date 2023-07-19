using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

// 이펙트는 오브젝트 풀링에서 꺼내서 적용

abstract public class BasicSkill : MonoBehaviour // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public Action<BasicSkill> RemoveFromList;

    [SerializeField]
    protected SkillData data; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    public SkillData Data { get { return data; } }

    protected BasicEffectPlayer effectPlayer;
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } }

    /// <summary>
    /// 스킬의 위치를 지정할 오브젝트
    /// </summary>
    Transform posTr;
    public Transform PosTr { get { return posTr; } set { posTr = value; } }

    [SerializeField]
    protected PositionMethod positionMethod; // 스킬의 위치 지정
    public PositionMethod PositionMethod { get { return positionMethod; } }

    // 원거리 공격, 근거리 공격 등등 공격 방식 설정 --> 시전 위치는 스킬 위치를 기준으로 함
    //[SerializeField]
    //protected DamageMethod damageMethod; 
    //public DamageMethod DamageMethod { get { return damageMethod; } }

    private void Update()
    {
        positionMethod.DoUpdate(this);
    }

    public virtual void Execute(GameObject caster) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        positionMethod.Init(caster.transform, this);
    }

    protected virtual void OnDisable()
    {
        posTr = null;
        
        if(RemoveFromList != null) RemoveFromList(this);
        ObjectPooler.ReturnToPool(gameObject);
    }
}

abstract public class AttackSkill : BasicSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    // 원거리 공격, 근거리 공격 등등 공격 방식 설정 --> 시전 위치는 스킬 위치를 기준으로 함
    [SerializeField]
    protected DamageMethod damageMethod;
    public DamageMethod DamageMethod { get { return damageMethod; } }

    [SerializeField]
    DrawGizmo drawGizmo;

    private void Update()
    {
        positionMethod.DoUpdate(this);
    }

    void OnDrawGizmos()
    {
        drawGizmo.DrawBoxGizmo(transform);
        drawGizmo.DrawCircleGizmo(transform);
    }
}

abstract public class TickSkill : AttackSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    protected CancellationTokenSource m_source = new();

    public void CancelTask()
    {
        if(m_source != null)
        {
            if(effectPlayer != null) effectPlayer.StopEffect();

            m_source.Cancel();
            m_source = null;
            m_source = new();
        }
    }

    protected void OnDestroy()
    {
        m_source.Cancel();
        m_source.Dispose();
    }

    protected void OnEnable()
    {
        if (m_source != null)
            m_source.Dispose();

        m_source = new();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_source.Cancel();
    }
}