using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

abstract public class BasicSkill : MonoBehaviour // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public Action<BasicSkill> RemoveFromList;

    [SerializeField]
    protected SkillData m_data; // --> ������, ���� ��� ����� ������ �־�����
    public SkillData Data { get { return m_data; } }

    /// <summary>
    /// ��ų�� ��ġ�� ������ ������Ʈ
    /// </summary>
    Transform posTr;
    public Transform PosTr { get { return posTr; } set { posTr = value; } }

    protected BasicEffectPlayer effectPlayer;
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } }

    [SerializeField]
    protected PositionMethod positionMethod; // ��ų�� ��ġ ����
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

    public virtual void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
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
        RemoveFromList = null; // �ʱ�ȭ
        posTr = null;
        ObjectPooler.ReturnToPool(gameObject);
    }
}

abstract public class AttackSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    // ���Ÿ� ����, �ٰŸ� ���� ��� ���� ��� ���� --> ���� ��ġ�� ��ų ��ġ�� �������� ��
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

abstract public class TickAttackSkill : AttackSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    protected CancellationTokenSource m_source = new();

    public virtual void QuickEndTask()
    {
        m_source.Cancel();
        m_source.Dispose();
        m_source = new(); // ����ϸ� �ٽ� �־��ֱ�
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

public class SpawnSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    [SerializeField]
    protected List<BasicProjectile> spawnedObjects = new List<BasicProjectile>();
    public List<BasicProjectile> SpawnedObjects { get { return spawnedObjects; } }

    // ���Ÿ� ����, �ٰŸ� ���� ��� ���� ��� ���� --> ���� ��ġ�� ��ų ��ġ�� �������� ��
    [SerializeField]
    protected SpawnMethod spawnMethod;
    public SpawnMethod SpawnMethod { get { return spawnMethod; } }
    // -- Method �ϳ� �� ���� ��ų�� ���� ���� ���� �ƴϸ� ���� ��ų�� �ѹ� �� ������ �Ǵ�

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

abstract public class TickSpawnSkill : SpawnSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    protected CancellationTokenSource m_source = new();

    public virtual void QuickEndTask()
    {
        m_source.Cancel();
        m_source.Dispose();
        m_source = new(); // ����ϸ� �ٽ� �־��ֱ�
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