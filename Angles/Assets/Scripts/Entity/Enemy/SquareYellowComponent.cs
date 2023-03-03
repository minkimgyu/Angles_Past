using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class SquareYellowComponent : UnitaskUtility
{
    Enemy enemy;
    FollowComponent followComponent;
    EnemyBattleComponent enemyBattleComponent;
    bool nowCanUseSkill = true;


    private void Start()
    {
        enemy = GetComponent<Enemy>();
        followComponent = GetComponent<FollowComponent>();
        enemyBattleComponent = GetComponent<EnemyBattleComponent>();

        followComponent.skillAction += SkillAction;
    }

    public void SkillAction()
    {
        if (nowCanUseSkill == false) return;

        nowCanUseSkill = false;
        followComponent.StopFollow();
        enemyBattleComponent.PlayWhenCondition();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        nowCanUseSkill = true;
        if (followComponent != null) followComponent.skillAction -= SkillAction;
    }
}
