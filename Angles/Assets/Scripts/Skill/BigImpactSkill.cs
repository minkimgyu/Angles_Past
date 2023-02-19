using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigImpactSkill : BasicSkill
{
    public Color color;
    public float radius;

    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        effect.PlayEffect();

        transform.position = skillSupportData.player.transform.position; // 위치 초기화

        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                hit[i].collider.GetComponent<FollowComponent>().WaitFollow();
                hit[i].collider.GetComponent<BasicReflectComponent>().KnockBack((hit[i].transform.position - transform.position).normalized * 6);
            }
        }

        base.PlaySkill(skillSupportData);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
