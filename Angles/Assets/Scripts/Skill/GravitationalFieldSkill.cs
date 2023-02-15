using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalFieldSkill : BasicSkill
{
    [SerializeField]
    List<ForceComponent> forceComponents = new List<ForceComponent>();

    protected override void OnEnable()
    {
        WhenEnable();
        Invoke("DisableObject", 15f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        { 
            forceComponents.Add(col.GetComponent<ForceComponent>());
            print(col);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) forceComponents.Remove(col.GetComponent<ForceComponent>());
    }

    private void Update()
    {
        for (int i = 0; i < forceComponents.Count; i++)
        {
            Vector3 dir = -(forceComponents[i].transform.position - transform.position).normalized * 15;
            forceComponents[i].AddForceUsingVec(dir, ForceMode2D.Force);
        }
    }

    public override void PlaySkill(Vector2 dir, List<Collision2D> entity)
    {
        GetEffectUsingName("GravitationalFieldEffect", transform.position, transform.rotation);
        base.PlaySkill(dir, entity);
    }
}
