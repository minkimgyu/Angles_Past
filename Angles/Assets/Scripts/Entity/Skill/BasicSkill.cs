using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

abstract public class BasicSkill : MonoBehaviour // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public Action<BasicSkill> RemoveFromList;

    [SerializeField]
    protected SkillData m_data; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    public SkillData Data { get { return m_data; } }

    /// <summary>
    /// 스킬의 위치를 지정할 오브젝트
    /// </summary>
    Transform posTr;
    public Transform PosTr { get { return posTr; } set { posTr = value; } }

    protected BasicEffectPlayer effectPlayer;
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } }

    [SerializeField]
    protected PositionMethod positionMethod; // 스킬의 위치 지정
    public PositionMethod PositionMethod { get { return positionMethod; } }

    protected virtual void Update()
    {
        positionMethod.DoUpdate(this);
    }

    public void Init(BattleComponent battleComponent, SkillData data)
    {
        m_data = data;
        RemoveFromList += battleComponent.RemoveFromActiveSkills;
    }

    public virtual void Execute(GameObject caster) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        positionMethod.Init(caster.transform, this);
    }

    public virtual void OnEnd()
    {
        m_data = null;
        RemoveFromList(this);
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        RemoveFromList = null; // 초기화
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

    [SerializeField]
    DrawGizmo drawGizmo;

    void OnDrawGizmos()
    {
        drawGizmo.DrawBoxGizmo(transform);
        drawGizmo.DrawCircleGizmo(transform);
    }

    public override void OnEnd()
    {
        effectPlayer = null;
        base.OnEnd();
    }
}

abstract public class TickAttackSkill : AttackSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    protected CancellationTokenSource m_source = new();

    public virtual void QuickEndTask()
    {
        m_source.Cancel();
        m_source.Dispose();
        m_source = new(); // 취소하면 다시 넣어주기
    }

    protected virtual void OnDestroy()
    {
        m_source.Cancel();
        m_source.Dispose();
    }

    protected virtual void OnEnable()
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

public class SpawnSkill : BasicSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    [SerializeField]
    protected List<BasicProjectile> spawnedObjects = new List<BasicProjectile>();
    public List<BasicProjectile> SpawnedObjects { get { return spawnedObjects; } }

    // 원거리 공격, 근거리 공격 등등 공격 방식 설정 --> 시전 위치는 스킬 위치를 기준으로 함
    [SerializeField]
    protected SpawnMethod spawnMethod;
    public SpawnMethod SpawnMethod { get { return spawnMethod; } }
    // -- Method 하나 더 만들어서 스킬이 새로 사용될 건지 아니면 기존 스킬을 한번 더 쓸건지 판단

    protected override void Update()
    {
        base.Update();

        positionMethod.DoUpdate(this);
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            spawnedObjects[i].DoUpdate();
            if (spawnedObjects[i].IsFinished)
            {
                spawnedObjects[i].OnEnd();
                spawnedObjects.Remove(spawnedObjects[i]);
                if (spawnedObjects.Count == 0)
                {
                    OnEnd();
                }
            }
        }
    }
}

abstract public class TickSpawnSkill : SpawnSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    protected CancellationTokenSource m_source = new();

    public virtual void QuickEndTask()
    {
        m_source.Cancel();
        m_source.Dispose();
        m_source = new(); // 취소하면 다시 넣어주기
    }

    protected virtual void OnDestroy()
    {
        m_source.Cancel();
        m_source.Dispose();
    }

    protected virtual void OnEnable()
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