using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TargetDesignation<T>
{
    protected bool isFindRangeSame;

    public abstract T Execute(SkillSupportData supportData);

    protected int ReturnTick(SkillSupportData supportData)
    {
        int tick = 0;
        if (!isFindRangeSame) tick = supportData.TickCount;

        return tick;
    }
}

public class FindInCircleRange : TargetDesignation<RaycastHit2D[]>
{
    List<float> m_radiusRangePerTick;
    List<Vector2> m_offsetRangePerTick;

    public FindInCircleRange(List<float> radiusRangePerTick) // 생성자에서 받아서 실행해준다.
    {
        m_radiusRangePerTick = radiusRangePerTick;
    }

    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Pos, m_radiusRangePerTick[ReturnTick(supportData)], 
            m_offsetRangePerTick[ReturnTick(supportData)].normalized, m_offsetRangePerTick[ReturnTick(supportData)].magnitude);

        return hit;
    }
}

public class FindInBoxRange : TargetDesignation<RaycastHit2D[]>
{
    List<Vector2> m_boxRangePerTick;
    List<Vector2> m_offsetRangePerTick;

    public FindInBoxRange(List<Vector2> boxRangePerTick, List<Vector2> offsetRangePerTick) // 생성자에서 받아서 실행해준다.
    {
        m_boxRangePerTick = boxRangePerTick;
        m_offsetRangePerTick = offsetRangePerTick;
    }

    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(supportData.Pos, m_boxRangePerTick[ReturnTick(supportData)],
            supportData.Caster.transform.rotation.z, m_offsetRangePerTick[ReturnTick(supportData)].normalized, m_offsetRangePerTick[ReturnTick(supportData)].magnitude);

        return hit;
    }
}

public class FindAllUsingRaycast : TargetDesignation<RaycastHit2D[]>
{
    List<Vector2> m_directionPerTick;
    List<float> m_rangePerTick;

    public FindAllUsingRaycast(List<Vector2> directionPerTick, List<float> rangePerTick) // 생성자에서 받아서 실행해준다.
    {
        m_directionPerTick = directionPerTick;
        m_rangePerTick = rangePerTick;
    }

    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(supportData.Pos, m_directionPerTick[ReturnTick(supportData)], m_rangePerTick[ReturnTick(supportData)]);

        Debug.DrawRay(supportData.Pos, m_directionPerTick[ReturnTick(supportData)].normalized * m_rangePerTick[ReturnTick(supportData)], Color.green, 100);

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

public class NoFound : TargetDesignation<GameObject>
{
    public override GameObject Execute(SkillSupportData supportData)
    {
        return null;
    }
}