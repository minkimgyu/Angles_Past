using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class HexagonYellowComponent : UnitaskUtility
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

        followComponent.StopFollow();

        enemyBattleComponent.PlayWhenCondition();
        await UniTask.Delay(TimeSpan.FromSeconds(enemy.enemyData.SkillReuseTime), cancellationToken: BasicTask.source.Token);

        followComponent.ResetFollow();

        nowCanUseSkill = true;
        BasicTask.NowRunning = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(followComponent != null) followComponent.skillAction -= SkillAction;
    }
}
