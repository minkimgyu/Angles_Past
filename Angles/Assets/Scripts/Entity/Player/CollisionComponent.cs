using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionComponent : MonoBehaviour
{
    Player player;
    AttackComponent attackComponent;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        attackComponent = GetComponent<AttackComponent>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player.PlayerMode != PlayerMode.Attack) return;

        if (collision.gameObject.CompareTag("Wall") == true)
        {
            WallColorChange colorChange = collision.gameObject.GetComponent<WallColorChange>();
            colorChange.ChangeTileColor(ReturnHitPosition(collision));

            attackComponent.CancelTask();
            attackComponent.PlayAttack(ReflectPlayer(collision.contacts[0].normal) * DatabaseManager.Instance.ReflectAttackThrust);
        }
    }

    Vector3 ReturnHitPosition(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
        }

        return hitPosition;
    }

    Vector2 ReflectPlayer(Vector2 hitPoint)
    {
        Vector2 velocity = player.rigid.velocity;
        var dir = Vector2.Reflect(velocity.normalized, hitPoint);
        return dir;
    }
}