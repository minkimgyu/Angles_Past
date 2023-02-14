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

            if (entity[i].gameObject.name == "Player") return;

            FollowComponent followComponent = entity[i].gameObject.GetComponent<FollowComponent>();
            
            followComponent.nowHit = true;
            followComponent.WaitFollow();

            if(followComponent.closeEnemy.Count == 0)
            {
                entity[i].gameObject.GetComponent<BasicReflectComponent>().KnockBack(dirToEnemy.normalized);
            }
            else
            {
                entity[i].gameObject.GetComponent<BasicReflectComponent>().KnockBack(dirToEnemy.normalized * followComponent.closeEnemy.Count * 3);
            }

            GetEffectUsingName("NormalKnockBackEffect", entity[i].contacts[0].point, Quaternion.identity);
        }

        base.PlaySkill(dir, entity);
    }
}
