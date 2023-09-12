using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
abstract public class TargetDesignation<T>
{
    public TargetDesignation() // �����ڿ��� �޾Ƽ� �������ش�.
    {
    }

    public abstract T Execute(SkillSupportData supportData);
}

public class FindInCircleRange : TargetDesignation<RaycastHit2D[]>
{
    float m_targetFindRange;
    float[] m_skillScalePerTicks;
    Vector2[] m_offsetRangePerTick;

    public FindInCircleRange(float targetFindRange, float[] skillScalePerTicks, Vector2[] offsetRangePerTick) // �����ڿ��� �޾Ƽ� �������ش�.
    {
        m_targetFindRange = targetFindRange;
        m_skillScalePerTicks = skillScalePerTicks;
        m_offsetRangePerTick = offsetRangePerTick;
    }

    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        DrawDebugCircle(supportData.Pos + (Vector3)m_offsetRangePerTick[supportData.TickCount].normalized, m_targetFindRange * m_skillScalePerTicks[supportData.TickCount], 3);

        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Pos, m_targetFindRange * m_skillScalePerTicks[supportData.TickCount], 
            m_offsetRangePerTick[supportData.TickCount].normalized, m_offsetRangePerTick[supportData.TickCount].magnitude);

        return hit;
    }

    void DrawDebugCircle(Vector3 center, float radius, float duration)
    {
        int segments = 50; // ���� �󸶳� �ε巴�� �׸����� ���ϴ� ���׸�Ʈ ��.
        float angleIncrement = 360f / segments;
        Vector3 prevPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleIncrement;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            Vector3 currentPoint = new Vector3(x, y, 0) + center;

            if (i > 0)
            {
                Debug.DrawLine(prevPoint, currentPoint, Color.red, duration);
            }

            prevPoint = currentPoint;
        }
    }
}

public class FindInBoxRange : TargetDesignation<RaycastHit2D[]>
{
    Vector2[] m_boxRangePerTick;
    Vector2[] m_offsetRangePerTick;

    public FindInBoxRange(Vector2[] boxRangePerTick, Vector2[] offsetRangePerTick) // �����ڿ��� �޾Ƽ� �������ش�.
    {
        m_boxRangePerTick = boxRangePerTick;
        m_offsetRangePerTick = offsetRangePerTick;
    }

    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        Debug.Log(supportData.Caster.transform.rotation.eulerAngles.z);


        DrawDebugBoxWithRotation(supportData.Pos + (Vector3)m_offsetRangePerTick[supportData.TickCount].normalized, m_boxRangePerTick[supportData.TickCount], supportData.Caster.transform.rotation.eulerAngles.z, 3);


        

        RaycastHit2D[] hit = Physics2D.BoxCastAll(supportData.Pos, m_boxRangePerTick[supportData.TickCount],
            supportData.Caster.transform.rotation.z, m_offsetRangePerTick[supportData.TickCount].normalized, m_offsetRangePerTick[supportData.TickCount].magnitude);

        return hit;
    }

    void DrawDebugBoxWithRotation(Vector3 center, Vector3 size, float rotationZ, float duration)
    {
        // ������ �������� ��ȯ�մϴ�.
        float radians = rotationZ * Mathf.Deg2Rad;

        // �簢���� �� ������ ��ǥ ���.
        Vector3 topLeft = center + Quaternion.Euler(0, 0, radians) * new Vector3(-size.x / 2, size.y / 2, 0);
        Vector3 topRight = center + Quaternion.Euler(0, 0, radians) * new Vector3(size.x / 2, size.y / 2, 0);
        Vector3 bottomLeft = center + Quaternion.Euler(0, 0, radians) * new Vector3(-size.x / 2, -size.y / 2, 0);
        Vector3 bottomRight = center + Quaternion.Euler(0, 0, radians) * new Vector3(size.x / 2, -size.y / 2, 0);

        // �簢���� ���� �׸��ϴ�.
        Debug.DrawLine(topLeft, topRight, Color.blue, duration);
        Debug.DrawLine(topRight, bottomRight, Color.blue, duration);
        Debug.DrawLine(bottomRight, bottomLeft, Color.blue, duration);
        Debug.DrawLine(bottomLeft, topLeft, Color.blue, duration);
    }
}

public class FindAllUsingRaycast : TargetDesignation<RaycastHit2D[]>
{
    Vector3[] m_directionPerTick;
    float[] m_rangePerTick;

    public FindAllUsingRaycast(float[] rangePerTick) // �����ڿ��� �޾Ƽ� �������ش�.
    {
        m_rangePerTick = rangePerTick;
    }

    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        m_directionPerTick = supportData.Caster.GetComponent<ISpecifyDirection>().Directions; // ������ ���� GetComponent�� �̿��ؼ� �޾ƿ´�.
        RaycastHit2D[] hits = Physics2D.RaycastAll(supportData.Pos, m_directionPerTick[supportData.TickCount], m_rangePerTick[supportData.TickCount]);

        Debug.DrawLine(supportData.Pos, m_directionPerTick[supportData.TickCount].normalized * m_rangePerTick[supportData.TickCount], Color.green, 100);

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

public class FindInTriggered : TargetDesignation<List<GameObject>>
{
    bool nowApply;

    public FindInTriggered(bool nowApply) // �����ڿ��� �޾Ƽ� �������ش�.
    {
        this.nowApply = nowApply;
    }

    public override List<GameObject> Execute(SkillSupportData supportData)
    {
        TriggerComponent triggerComponent = supportData.Caster.GetComponentInChildren<TriggerComponent>();
        if (triggerComponent == null) return null;

        return triggerComponent.ReturnTriggeredObjects(nowApply);
    }
}

public class FindPlayer : TargetDesignation<Transform>
{
    public override Transform Execute(SkillSupportData supportData)
    {
        return supportData.Caster.GetComponent<BaseFollowEnemy>().LoadPlayer.transform;
    }
}

public class NoFound : TargetDesignation<GameObject>
{
    public override GameObject Execute(SkillSupportData supportData)
    {
        return null;
    }
}