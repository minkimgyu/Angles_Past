using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : BasicEffect
{
    public Color color;
    public float radius;
    public Transform explodeTr;

    protected override void OnEnable()
    {
        if (canOffOverTime == true) Invoke("DisableObject", time);
        Invoke("InvokeEffect", 2f);
    }

    public void SetExplodePos(Transform tr)
    {
        explodeTr = tr;
    }

    protected override void OnDisable()
    {
        explodeTr = null;
        base.OnDisable();
    }

    void InvokeEffect()
    {
        if(explodeTr != null) transform.position = explodeTr.position;
        PlayEffect();
        DamageToRange();
    }

    public void DamageToRange()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                hit[i].collider.GetComponent<FollowComponent>().WaitFollow();
                hit[i].collider.GetComponent<BasicReflectComponent>().KnockBack((hit[i].transform.position - transform.position).normalized * 2);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
}
