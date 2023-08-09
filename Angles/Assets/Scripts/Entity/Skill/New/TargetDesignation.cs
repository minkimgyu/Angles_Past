using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TargetDesignation<T>
{
    public abstract T Execute(SkillSupportData supportData);
}

public class FindInCircleRange : TargetDesignation<RaycastHit2D[]>
{
    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Pos, supportData.Data.RadiusRangePerTick[supportData.TickCount], Vector2.up, 0);
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

public class NoFound : TargetDesignation<GameObject>
{
    public override GameObject Execute(SkillSupportData supportData)
    {
        return null;
    }
}