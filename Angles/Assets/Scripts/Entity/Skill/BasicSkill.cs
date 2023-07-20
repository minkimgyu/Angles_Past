using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

abstract public class BasicSkill : MonoBehaviour // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    [SerializeField]
    protected SkillData m_data; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    public SkillData Data { get { return m_data; } }

    /// <summary>
    /// 스킬의 위치를 지정할 오브젝트
    /// </summary>
    Transform posTr;
    public Transform PosTr { get { return posTr; } set { posTr = value; } }

    [SerializeField]
    private bool isFinished; // 스킬의 위치 지정
    public bool IsFinished 
    { 
        get { return isFinished; }
        set
        {
            if (disableOnself == false) return;
            OnEnd();
        }
    }

    bool disableOnself = false; // battleComponent가 먼저 사라질 경우 하위 스킬들은 종료시 알아서 제거됨
    public bool DisableOnself { set { disableOnself = value; } }

    [SerializeField]
    protected PositionMethod positionMethod; // 스킬의 위치 지정
    public PositionMethod PositionMethod { get { return positionMethod; } }

    public virtual void DoUpdate(float tick)
    {
        positionMethod.DoUpdate(this);
    }

    public void Init(SkillData data)
    {
        m_data = data;
        IsFinished = false;
    }

    public virtual void Execute(GameObject caster) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        positionMethod.Init(caster.transform, this);
    }

    public virtual void OnEnd()
    {
        disableOnself = false;
        m_data = null;
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        posTr = null;
        ObjectPooler.ReturnToPool(gameObject);
    }
}

abstract public class AttackSkill : BasicSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    // 원거리 공격, 근거리 공격 등등 공격 방식 설정 --> 시전 위치는 스킬 위치를 기준으로 함
    [SerializeField]
    protected DamageMethod damageMethod;
    public DamageMethod DamageMethod { get { return damageMethod; } }

    protected BasicEffectPlayer effectPlayer;
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } }

    [SerializeField]
    DrawGizmo drawGizmo;

    void OnDrawGizmos()
    {
        drawGizmo.DrawBoxGizmo(transform);
        drawGizmo.DrawCircleGizmo(transform);
    }

    public override void OnEnd()
    {
        if (effectPlayer != null) effectPlayer.StopEffect();
        effectPlayer = null;
        base.OnEnd();
    }
}

abstract public class TickAttackSkill : AttackSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    protected DamageSupportData damageSupportData;
    protected bool nowStart;

    [SerializeField]
    protected float storedDelay;

    public override void Execute(GameObject caster)
    {
        base.Execute(caster);
        CancelSkill();
        damageSupportData = new DamageSupportData(caster, this);
        nowStart = true;
    }

    public abstract void CancelSkill();

    public override void OnEnd()
    {
        storedDelay = 0;
        nowStart = false; // 스킬 종료
        base.OnEnd();
    }

    public override void DoUpdate(float tick)
    {
        base.DoUpdate(tick);
        if (nowStart == false) return;

        DamageTask(tick);
    }

    abstract protected void DamageTask(float tick);
}

abstract public class TickSpawnSkill : SpawnSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
   
}