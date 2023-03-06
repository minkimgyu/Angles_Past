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
        followComponent.skillAction += SkillAction;
    }

    public void SkillAction()
    {
        if (nowCanUseSkill == false) return;
        SkillTask().Forget();
    }

    public async UniTaskVoid SkillTask()
    {
        BasicTask.NowRunning = true;
        nowCanUseSkill = false;

        enemyBattleComponent.PlayWhenCondition();
        await UniTask.Delay(TimeSpan.FromSeconds(enemy.enemyData.SkillReuseTime), cancellationToken: BasicTask.source.Token);

        nowCanUseSkill = true;
        BasicTask.NowRunning = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BasicTask.CancelTask();
        if (followComponent != null) followComponent.skillAction -= SkillAction;
    }
}
