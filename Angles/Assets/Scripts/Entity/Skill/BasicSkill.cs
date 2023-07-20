using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

abstract public class BasicSkill : MonoBehaviour // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    [SerializeField]
    protected SkillData m_data; // --> ������, ���� ��� ����� ������ �־�����
    public SkillData Data { get { return m_data; } }

    /// <summary>
    /// ��ų�� ��ġ�� ������ ������Ʈ
    /// </summary>
    Transform posTr;
    public Transform PosTr { get { return posTr; } set { posTr = value; } }

    [SerializeField]
    private bool isFinished; // ��ų�� ��ġ ����
    public bool IsFinished 
    { 
        get { return isFinished; }
        set
        {
            if (disableOnself == false) return;
            OnEnd();
        }
    }

    bool disableOnself = false; // battleComponent�� ���� ����� ��� ���� ��ų���� ����� �˾Ƽ� ���ŵ�
    public bool DisableOnself { set { disableOnself = value; } }

    [SerializeField]
    protected PositionMethod positionMethod; // ��ų�� ��ġ ����
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

    public virtual void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
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

abstract public class AttackSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    // ���Ÿ� ����, �ٰŸ� ���� ��� ���� ��� ���� --> ���� ��ġ�� ��ų ��ġ�� �������� ��
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

abstract public class TickAttackSkill : AttackSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
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
        nowStart = false; // ��ų ����
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

abstract public class TickSpawnSkill : SpawnSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
   
}