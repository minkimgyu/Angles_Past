using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

// ����Ʈ�� ������Ʈ Ǯ������ ������ ����

abstract public class BasicSkill : MonoBehaviour // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    [SerializeField]
    protected SkillData data; // --> ������, ���� ��� ����� ������ �־�����
    public SkillData Data { get { return data; } }

    /// <summary>
    /// ��ų�� ��ġ�� ������ ������Ʈ
    /// </summary>
    Transform posTr;
    public Transform PosTr { get { return posTr; } set { posTr = value; } }

    [SerializeField]
    protected PositionMethod positionMethod; // ��ų�� ��ġ ����
    public PositionMethod PositionMethod { get { return positionMethod; } }

    // ���Ÿ� ����, �ٰŸ� ���� ��� ���� ��� ���� --> ���� ��ġ�� ��ų ��ġ�� �������� ��
    [SerializeField]
    protected DamageMethod damageMethod; 
    public DamageMethod DamageMethod { get { return damageMethod; } }

    [SerializeField]
    DrawGizmo drawGizmo;

    private void Update()
    {
        positionMethod.DoUpdate(this);
    }

    public virtual void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        positionMethod.Init(caster.transform, this);
    }

    private void OnDisable()
    {
        posTr = null;
        ObjectPooler.ReturnToPool(gameObject);
    }

    void OnDrawGizmos()
    {
        drawGizmo.DrawBoxGizmo(transform);
        drawGizmo.DrawCircleGizmo(transform);
    }
}

abstract public class TickSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    protected CancellationTokenSource m_source = new();

    public void CancelTask()
    {
        m_source.Cancel();
        m_source = null;
        m_source = new();
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

    protected void OnDisable()
    {
        m_source.Cancel();
    }
}