using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectComponent : MonoBehaviour
{
    Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public Vector2 ResetReflectVec(Vector2 hitPoint) // �ݻ� ����
    {
        Vector2 velocity = rigid.velocity;
        return Vector2.Reflect(velocity.normalized, hitPoint).normalized;
    }

    //Vector3 ReturnHitPosition(Collision2D collision) // �� �浹 ��ġ ã�� --> ���� �־ ��� ���� �� ����
    //{
    //    Vector3 hitPosition = Vector3.zero;
    //    foreach (ContactPoint2D hit in collision.contacts)
    //    {
    //        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
    //        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
    //    }

    //    return hitPosition;
    //}
}
