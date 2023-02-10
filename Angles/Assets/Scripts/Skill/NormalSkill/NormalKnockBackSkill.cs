using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalKnockBackSkill : BasicSkill
{
    public override void PlaySkill(Vector2 dir, List<Collision2D> entity)
    {
        for (int i = 0; i < entity.Count; i++)
        {
            Vector2 dirToEnemy = entity[i].transform.position - transform.position;

            print(entity[i].gameObject.name);

            entity[i].gameObject.GetComponent<FollowComponent>().WaitFollow();
            entity[i].gameObject.GetComponent<BasicReflectComponent>().KnockBack(dirToEnemy.normalized * 4);
            GetEffectUsingName(entity[i].contacts[0].point, Quaternion.identity);
        }
    }
}
