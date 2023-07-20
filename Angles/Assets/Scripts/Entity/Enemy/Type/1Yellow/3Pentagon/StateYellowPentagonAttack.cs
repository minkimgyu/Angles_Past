using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StateYellowPentagonAttack : StateFollowEnemyAttack
{
    YellowPentagonEnemy loadYellowPentagonEnemy;

    CancellationTokenSource _source = new();
    bool _nowAttack = false;

    public StateYellowPentagonAttack(YellowPentagonEnemy yellowPentagonEnemy) : base(yellowPentagonEnemy)
    {
        loadYellowPentagonEnemy = yellowPentagonEnemy;
    }

    // 계속 발사해야해서 unitask로 제작
    public override void ExecuteInRangeMethod()
    {
        AttackTask().Forget();
    }

    private async UniTaskVoid AttackTask()
    {
        while(true)
        {
            loadYellowPentagonEnemy.BattleComponent.UseSkill(SkillUseConditionType.InRange);
            await UniTask.Delay(TimeSpan.FromSeconds(loadYellowPentagonEnemy.Delay), cancellationToken: _source.Token);
        }
    }

    public override void ExecuteInOutsideMethod()
    {
        QuickEndTask();
    }

    public void QuickEndTask()
    {
        _source.Cancel();
        _source.Dispose();
        _source = new(); // 취소하면 다시 넣어주기
    }

    private void OnDestroy()
    {
        _source.Cancel();
        _source.Dispose();
    }

    private void OnEnable()
    {
        if (_source != null)
            _source.Dispose();

        _source = new();
    }

    private void OnDisable()
    {
        _source.Cancel();
    }
}
