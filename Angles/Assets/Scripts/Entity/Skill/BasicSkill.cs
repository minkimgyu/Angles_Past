using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

// ����Ʈ�� ������Ʈ Ǯ������ ������ ����

abstract public class BasicSkill : MonoBehaviour // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public Action<BasicSkill> RemoveFromList;

    [SerializeField]
    protected SkillData data; // --> ������, ���� ��� ����� ������ �־�����
    public SkillData Data { get { return data; } }

    protected BasicEffectPlayer effectPlayer;
    public BasicEffectPlayer EffectPlayer { get { return effectPlayer; } set { effectPlayer = value; } }

    /// <summary>
    /// ��ų�� ��ġ�� ������ ������Ʈ
    /// </summary>
    Transform posTr;
    public Transform PosTr { get { return posTr; } set { posTr = value; } }

    [SerializeField]
    protected PositionMethod positionMethod; // ��ų�� ��ġ ����
    public PositionMethod PositionMethod { get { return positionMethod; } }

    // ���Ÿ� ����, �ٰŸ� ���� ��� ���� ��� ���� --> ���� ��ġ�� ��ų ��ġ�� �������� ��
    //[SerializeField]
    //protected DamageMethod damageMethod; 
    //public DamageMethod DamageMethod { get { return damageMethod; } }

    private void Update()
    {
        positionMethod.DoUpdate(this);
    }

    public virtual void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
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

abstract public class AttackSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
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

    void OnDrawGizmos()
    {
        drawGizmo.DrawBoxGizmo(transform);
        drawGizmo.DrawCircleGizmo(transform);
    }
}

abstract public class TickSkill : AttackSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
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