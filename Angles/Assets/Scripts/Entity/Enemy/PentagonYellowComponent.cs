using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PentagonYellowComponent : UnitaskUtility
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
        followComponent.followAction += SkillAction;
    }

    public void SkillAction()
    {
        if (nowCanUseSkill == false) return;
        SkillTask().Forget();
    }

    public async UniTaskVoid SkillTask()
    {
        nowRunning = true;
        nowCanUseSkill = false;
        followComponent.WaitFollow();

        await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: source.Token);
        enemyBattleComponent.PlayWhenCondition();

        nowCanUseSkill = true;
        nowRunning = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        followComponent.followAction -= SkillAction;
    }
}