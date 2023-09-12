using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
abstract public class TargetDesignation<T>
{
    public TargetDesignation() // 생성자에서 받아서 실행해준다.
    {
    }

    public abstract T Execute(SkillSupportData supportData);
}

public class FindInCircleRange : TargetDesignation<RaycastHit2D[]>
{
    float m_targetFindRange;
    float[] m_skillScalePerTicks;
    Vector2[] m_offsetRangePerTick;

    public FindInCircleRange(float targetFindRange, float[] skillScalePerTicks, Vector2[] offsetRangePerTick) // 생성자에서 받아서 실행해준다.
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
        int segments = 50; // 원을 얼마나 부드럽게 그릴지를 정하는 세그먼트 수.
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

    public FindInBoxRange(Vector2[] boxRangePerTick, Vector2[] offsetRangePerTick) // 생성자에서 받아서 실행해준다.
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
        // 각도를 라디안으로 변환합니다.
        float radians = rotationZ * Mathf.Deg2Rad;

        // 사각형의 네 꼭짓점 좌표 계산.
        Vector3 topLeft = center + Quaternion.Euler(0, 0, radians) * new Vector3(-size.x / 2, size.y / 2, 0);
        Vector3 topRight = center + Quaternion.Euler(0, 0, radians) * new Vector3(size.x / 2, size.y / 2, 0);
        Vector3 bottomLeft = center + Quaternion.Euler(0, 0, radians) * new Vector3(-size.x / 2, -size.y / 2, 0);
        Vector3 bottomRight = center + Quaternion.Euler(0, 0, radians) * new Vector3(size.x / 2, -size.y / 2, 0);

        // 사각형의 변을 그립니다.
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

    public FindAllUsingRaycast(float[] rangePerTick) // 생성자에서 받아서 실행해준다.
    {
        m_rangePerTick = rangePerTick;
    }

    public override RaycastHit2D[] Execute(SkillSupportData supportData)
    {
        m_directionPerTick = supportData.Caster.GetComponent<ISpecifyDirection>().Directions; // 다음과 같이 GetComponent를 이용해서 받아온다.
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

    public FindInTriggered(bool nowApply) // 생성자에서 받아서 실행해준다.
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