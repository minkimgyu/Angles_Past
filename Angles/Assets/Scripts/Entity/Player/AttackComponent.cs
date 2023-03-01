using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class AttackComponent : ForceComponent
{
    MoveComponent moveComponent;
    BattleComponent battleComponent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveComponent = GetComponent<MoveComponent>();
        battleComponent = GetComponent<BattleComponent>();
        entity.fixedUpdateAction += SubUpdate;

        PlayManager.Instance.actionJoy.actionComponent.aniAction += ResetReadyAni;
        PlayManager.Instance.actionJoy.actionComponent.attackAction += AddForceUsingVec;
    }
    void ResetReadyAni(bool nowReady) => entity.Animator.SetBool("NowReady", nowReady);

    void SubUpdate()
    {
        if (entity.PlayerMode != ActionMode.Attack) return;

        if (PlayManager.Instance.moveJoy.moveInputComponent.NowCancelAttack() == true) // 현재 공격을 취소해야할 경우
        {
            CancelTask();
            CancelAttack();
        }
    }

    public override void AddForceUsingVec(Vector2 attackDir, ForceMode2D forceMode = ForceMode2D.Impulse)
    {
        if (entity.PlayerMode != ActionMode.Idle && entity.PlayerMode != ActionMode.Attack) return;

        StopEntity();

        PlayManager.Instance.moveJoy.moveInputComponent.SetLoadVec(); // 현재 벡터를 저장해둔다.

        ResetReadyAni(false);
        entity.Animator.SetBool("NowAttack", true);

        base.AddForceUsingVec(attackDir);

        moveComponent.RotateUsingTransform(entity.rigid.velocity);

        entity.PlayerMode = ActionMode.Attack;
        WaitAttackEndTask().Forget();
    }

    public async UniTaskVoid WaitAttackEndTask()
    {
        nowRunning = true;
        entity.rigid.freezeRotation = true;

        await UniTask.Delay(TimeSpan.FromSeconds(DatabaseManager.Instance.PlayerData.RushTime), cancellationToken: source.Token);
        CancelAttack();

        nowRunning = false;
    }

    public void CancelAttack()
    {
        entity.PlayerMode = ActionMode.Idle;
        entity.Animator.SetBool("NowAttack", false);
        entity.rigid.freezeRotation = false;
    }

    public void QuickEndTask()
    {
        CancelTask();
        CancelAttack();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        entity.fixedUpdateAction -= SubUpdate;
        if (PlayManager.Instance == null) return;

        PlayManager.Instance.actionJoy.actionComponent.aniAction -= ResetReadyAni;
        PlayManager.Instance.actionJoy.actionComponent.attackAction -= AddForceUsingVec;
    }
}
