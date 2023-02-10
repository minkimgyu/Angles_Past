using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombSkill : BasicSkill
{
    protected override void OnEnable()
    {
        base.OnEnable();
        skillUseCount = 2; // »ç¿ëÈ½¼ö 2¹ø
    }

    public override void PlaySkill(Vector2 dir, List<Collision2D> entity)
    {
        GameObject effectGo = GetEffectUsingName(transform.position, transform.rotation, transform);
        effectGo.GetComponent<ExplosionEffect>().SetExplodePos(entity[0].transform);

        base.PlaySkill(dir, entity);
    }
}
