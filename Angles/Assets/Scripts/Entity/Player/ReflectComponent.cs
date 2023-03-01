using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectComponent : BasicReflectComponent
{
    public ActionMode actionMode;
    public string actionTag;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        ReflectEntity(col);
    }

    public void ReflectEntity(Collision2D col)
    {
        if (entity.PlayerMode != actionMode) return;

        if (col.gameObject.CompareTag(actionTag) == true)
        {
            col.gameObject.GetComponent<WallColorChange>().ChangeTileColor(ReturnHitPosition(col));
            KnockBack(ReflectPlayer(col.contacts[0].normal) * DatabaseManager.Instance.PlayerData.ReflectThrust);
        }
    }
    protected Vector2 ReflectPlayer(Vector2 hitPoint)
    {
        Vector2 velocity = entity.rigid.velocity;
        var dir = Vector2.Reflect(velocity.normalized, hitPoint);
        return dir;
    }

    Vector3 ReturnHitPosition(Collision2D collision) // 벽 충돌용
    {
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
        }

        return hitPosition;
    }
}
