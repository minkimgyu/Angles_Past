using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleComponent : BasicBattleComponent
{
    public void PlayWhenCondition()
    {
        UseSkill(SkillUseType.Condition);
    }
}
