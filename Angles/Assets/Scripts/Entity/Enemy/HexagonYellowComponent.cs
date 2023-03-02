using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class HexagonYellowComponent : UnitaskUtility
{
    FollowComponent followComponent;
    EnemyBattleComponent enemyBattleComponent;
    bool nowCanUseSkill = true;

    private void Start()
    {
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
        BasicTask.NowRunning = true;

        nowCanUseSkill = false;
        followComponent.StopFollow();

        enemyBattleComponent.PlayWhenCondition();
        await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: BasicTask.source.Token);

        nowCanUseSkill = true;
        BasicTask.NowRunning = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        followComponent.followAction -= SkillAction;
    }
}
