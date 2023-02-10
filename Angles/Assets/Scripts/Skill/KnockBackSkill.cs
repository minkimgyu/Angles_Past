using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackSkill : BasicSkill
{
    public Color color;
    public float boxWidth;
    public float boxHeight;
    public Vector3 distanceFromPlayer;

    public override void PlaySkill(Vector2 dir, List<Collision2D> entity)
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(boxWidth, boxHeight), 
            transform.rotation.z, Vector2.right, distanceFromPlayer.x, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                hit[i].collider.GetComponent<FollowComponent>().WaitFollow();
                hit[i].collider.GetComponent<BasicReflectComponent>().KnockBack(dir.normalized * 2);
            }
        }

        GetEffectUsingName(transform.position, transform.rotation);
        base.PlaySkill(dir, entity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(Vector3.zero + distanceFromPlayer, new Vector3(boxWidth, boxHeight, 0));
    }
}