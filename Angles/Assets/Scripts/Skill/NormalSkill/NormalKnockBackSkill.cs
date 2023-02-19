using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalKnockBackSkill : BasicSkill
{
    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        effect.PlayEffect();

        for (int i = 0; i < skillSupportData.contactEntity.Count; i++)
        {
            Vector2 dirToEnemy = skillSupportData.contactEntity[i].transform.position - transform.position;
            FollowComponent followComponent = skillSupportData.contactEntity[i].gameObject.GetComponent<FollowComponent>();
            
            followComponent.nowHit = true;
            followComponent.WaitFollow();

            if(followComponent.closeEnemy.Count == 0)
            {
                skillSupportData.contactEntity[i].gameObject.GetComponent<BasicReflectComponent>().KnockBack(dirToEnemy.normalized);
            }
            else
            {
                skillSupportData.contactEntity[i].gameObject.GetComponent<BasicReflectComponent>().KnockBack(dirToEnemy.normalized * followComponent.closeEnemy.Count * 3);
            }
        }

        base.PlaySkill(skillSupportData);
    }
}
