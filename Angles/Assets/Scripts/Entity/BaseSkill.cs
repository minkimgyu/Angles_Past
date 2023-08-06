using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SkillSupportData // --> 추후에 버프 추가
{
    public SkillSupportData(GameObject caster, SkillData data, Vector3 pos, int tickCount)//, int tickCount)
    {
        m_caster = caster;
        m_data = data;
        m_Pos = pos;
        m_tickCount = tickCount;
    }

    GameObject m_caster;
    public GameObject Caster { get { return m_caster; } }

    SkillData m_data;
    public SkillData Data { get { return m_data; } }

    Vector3 m_Pos;
    public Vector3 Pos { get { return m_Pos; } }

    int m_tickCount;
    public int TickCount { get { return m_tickCount; } }
}


abstract public class SpecifyLocation
{
    protected Transform m_posTr;
    protected Vector3 m_pos;
    protected bool m_isFix;

    protected GameObject m_caster;
    public GameObject Caster { get{ return m_caster; } }

    public virtual void Init(Transform caster, bool isFix)
    {
        m_caster = caster.gameObject;
    }

    public virtual Vector3 ReturnPos()
    {
        if (m_isFix) return m_posTr.position;
        else return m_pos;
    }
}

abstract public class LocationToCaster : SpecifyLocation
{
    public override void Init(Transform caster, bool isFix)
    {
        base.Init(caster, isFix);

        m_isFix = isFix;

        if (m_isFix) m_posTr = caster;
        else m_pos = caster.position;
    }
}

abstract public class LocationToContactor : SpecifyLocation
{
    public override void Init(Transform caster, bool isFix)
    {
        base.Init(caster, isFix);

        caster.TryGetComponent(out ContactComponent contact);
        if (contact == null) return;

        List<ContactData> supportData = contact.ReturnContactSupportData();

        m_isFix = isFix;

        if (m_isFix) m_posTr = supportData[0].tr;
        else m_pos = supportData[0].tr.position;
    }
}

abstract public class BaseMethod<T>
{
    List<BasicEffectPlayer> effectPlayers;

    public abstract void Execute(SkillSupportData supportData, T value);

    protected bool PlayEffect(Transform target, SkillData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.Name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = true;
        player.Init(target, data.DisableTime);
        player.PlayEffect();

        effectPlayers.Add(player);
        return true;
    }

    protected bool PlayEffect(Vector3 pos, SkillData data)
    {
        BasicEffectPlayer player = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(data.Name); // 이팩트 이름 추가
        if (player == null) return false;

        player.IsFixed = false;
        player.Init(pos, data.DisableTime);
        player.PlayEffect();

        effectPlayers.Add(player);
        return true;
    }

    protected void StopEffect()
    {
        for (int i = 0; i < effectPlayers.Count; i++)
        {
            effectPlayers[i].StopEffect();
            effectPlayers[i] = null;
        }
    }
}

abstract public class DamageMethod<T> : BaseMethod<T>
{
    protected bool DamageToEntity(GameObject me, Transform enemy, SkillData data)
    {
        enemy.TryGetComponent(out IHealth health);

        if (health == null || data.CanHitSkill(health.ReturnEntityTag()) == false) return false;

        health.UnderAttack(health.ReturnHealthEntityData(), data.Damage, -(me.transform.position - enemy.position).normalized, data.KnockBackThrust);
        return true;
    }
}

public class DamageToContactors : DamageMethod<List<ContactData>>
{
    public override void Execute(SkillSupportData supportData, List<ContactData> contactData)
    {
        for (int i = 0; i < contactData.Count; i++)
        {
            DamageToEntity(supportData.Caster, contactData[i].tr, supportData.Data); // 이렇게 진행
            PlayEffect(contactData[i].tr, supportData.Data);
        }
    }
}

public class DamageToRaycastHit : DamageMethod<RaycastHit2D[]>
{
    public override void Execute(SkillSupportData supportData, RaycastHit2D[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            DamageToEntity(supportData.Caster, hits[i].transform, supportData.Data); // 이렇게 진행
        }

        PlayEffect(supportData.Caster.transform, supportData.Data);
    }
}

public class DamageToLaserHit : DamageMethod<RaycastHit2D[]>
{
    [SerializeField]
    float hitEffectDisableTime = 1.5f;

    [SerializeField]
    float maxDistance = 20;

    [SerializeField]
    List<string> blockedTag = new List<string>();

    public override void Execute(SkillSupportData supportData, RaycastHit2D[] hits)
    {
        blockedTag.Add("Wall");

        List<Vector3> hitPos = new List<Vector3>();
        List<Vector3> hitEffectPos = new List<Vector3>();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == null || hits[i].transform == supportData.Caster.transform) continue;

            bool isBlocked = false;

            for (int j = 0; j < blockedTag.Count; j++)
            {
                if (hits[i].transform.tag == blockedTag[j])
                {
                    hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
                    hitEffectPos.Add(hits[i].point);
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked) break;

            if (DamageToEntity(supportData.Caster, hits[i].transform, supportData.Data) == false) continue;

            hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
            hitEffectPos.Add(hits[i].point);

            break;
        }

        if (hitPos.Count == 0)
        {
            hitPos.Add(supportData.Data.Directions[supportData.TickCount - 1] * maxDistance / 2);
        }

        for (int i = 0; i < hitEffectPos.Count; i++)
        {
            PlayEffect(hitEffectPos[i], supportData.Data);
        }

        PlayEffect(supportData.Caster.transform, supportData.Data);
        SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, supportData.Data.SfxName, supportData.Data.Volume);
    }
}
public interface ISkill
{
    public bool CheckIsFinish();

    public void Init(Transform caster);
    public void Execute();
    public void End();
}

public class BaseSkill<T> : ISkill
{
    [SerializeField]
    protected SkillData data; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    public SkillData Data { get { return data; } }

    SpecifyLocation specifyLocation;
    public SpecifyLocation SpecifyLocation { get { return specifyLocation; } }

    TargetDesignation<T> targetDesignation;
    public TargetDesignation<T> TargetDesignation { get { return targetDesignation; } }

    [SerializeField]
    List<BaseMethod<T>> baseMethods;

    public BaseSkill(SpecifyLocation specifyLocation, TargetDesignation<T> targetDesignation, List<BaseMethod<T>> baseMethods)
    {
        this.specifyLocation = specifyLocation;
        this.targetDesignation = targetDesignation;
        this.baseMethods = baseMethods;
    }

    float m_preDelay = 0;

    int m_tickCount = 0;

    float m_storedDuration = 0;

    bool m_nowFinish = false;
    public bool NowFinish { get { return m_nowFinish; } }

    public virtual void Init(Transform caster)
    {
        specifyLocation.Init(caster, true); // IsFix는 data 변수로 지정
    }

    public virtual void Execute() // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        float tickDuration = data.Duration / data.TickCount;
        if (data.PreDelay >= m_preDelay)
        {
            m_preDelay += Time.deltaTime;
            return;
        }

        if (data.TickCount > m_tickCount)
        {
            SkillRoutine(data.TickCount);

            m_storedDuration += Time.deltaTime;
            if (m_storedDuration > tickDuration)
            {
                m_storedDuration = 0;
                m_tickCount += 1;
            }
            else
            {
                m_nowFinish = true;
            }
        }
    }

    public virtual void SkillRoutine(int tickCount)
    {
        Vector3 myPos = specifyLocation.ReturnPos();

        SkillSupportData supportData = new SkillSupportData(specifyLocation.Caster, data, myPos, tickCount);
        T value = targetDesignation.Execute(supportData);

        for (int i = 0; i < baseMethods.Count; i++)
        {
            baseMethods[i].Execute(supportData, value);
        }
    }

    public virtual void End()
    {
        // 종료 시퀀스
        m_preDelay = 0;
        m_tickCount = 0;
        m_nowFinish = false;
    }
     
    public bool CheckIsFinish()
    {
        return NowFinish;
    }
}

public class StickyBomb : BaseSkill<RaycastHit2D[]>
{
    public StickyBomb(SpecifyLocation specifyLocation, TargetDesignation<RaycastHit2D[]> targetDesignation, List<BaseMethod<RaycastHit2D[]>> baseMethods) 
        : base(specifyLocation, targetDesignation, baseMethods) { }
}

abstract public class TargetDesignation<T>
{
    public abstract T Execute(SkillSupportData supportData);
}

public class FindInCircleRange : TargetDesignation<RaycastHit2D[]>
{
    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Pos, supportData.Data.RadiusRange, Vector2.up, 0);
        return hit;
    }
}

public class FindInBoxRange : TargetDesignation<RaycastHit2D[]>
{
    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(supportData.Pos, supportData.Data.BoxRange,
            supportData.Caster.transform.rotation.z, Vector2.right, supportData.Data.OffsetRange.magnitude);
        return hit;
    }
}

public class FindAllUsingRaycast : TargetDesignation<RaycastHit2D[]>
{
    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(supportData.Pos, supportData.Data.Directions[supportData.TickCount - 1], 100);
        Debug.DrawRay(supportData.Pos, supportData.Data.Directions[supportData.TickCount - 1].normalized, Color.green, 100);

        return hits;
    }
}

public class FindInContacted : TargetDesignation<List<ContactData>>
{
    public override List<ContactData> Execute(SkillSupportData supportData)
    {
        supportData.Caster.TryGetComponent(out ContactComponent contact);
        if (contact == null) return null;

        return contact.ReturnContactSupportData();
    }
}

