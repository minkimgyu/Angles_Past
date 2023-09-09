using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBossTriangleEnemyAttack : StateFollowEnemyAttack
{
    BossTriangleEnemy bossEnemy;

    public StateBossTriangleEnemyAttack(BossTriangleEnemy bossEnemy) : base(bossEnemy)
    {
        this.bossEnemy = bossEnemy;
    }

    public override void ReceiveTriggerEnter(Collider2D collider)
    {
        if(collider.gameObject.tag == "Enemy")
        {
            bossEnemy.TriggerComponent.CallWhenTriggerEnter(collider);
            loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);
        }
    }

    public override void ReceiveTriggerExit(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            bossEnemy.TriggerComponent.CallWhenTriggerExit(collider);
            loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.OutRange);
        }
    }

    public override void ExecuteInRangeMethod()
    {
    }

    public override void ExecuteInOutsideMethod()
    {
    }
}
