using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttackReady : BaseState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerAttackReady(Player player)
    {
        m_loadPlayer = player;
    }

    // 다른 state애서 변환되어 오는 경우 작동하는 함수
    public override void OnMessage(Telegram<Player.State> telegram)
    {
    }

    void Attack()
    {
        if (!m_loadPlayer.CheckOverMinValue(m_loadPlayer.ActionVec)) return;

        Message<Player.State> message = new Message<Player.State>();
        message.dir = -m_loadPlayer.ActionVec;
        Telegram<Player.State> telegram = new Telegram<Player.State>(Player.State.AttackReady, Player.State.Attack, message);

        SoundManager.Instance.PlaySFX(m_loadPlayer.transform.position, "Rush", 0.3f);
        m_loadPlayer.SetState(Player.State.Attack, telegram);
    }

    public override void OperateEnter()
    {
        m_loadPlayer.ActionJoystick.AttackAction += Attack;
        m_loadPlayer.ActionJoystick.AttackReadyAction += GoToMove;

        m_loadPlayer.Animator.SetBool("NowReady", true);
        m_loadPlayer.ResetRushRatioToZero();

        m_loadPlayer.shootDirection.gameObject.SetActive(true);
    }

    void GoToMove()
    {
        if (m_loadPlayer.CheckOverMinValue(m_loadPlayer.ActionVec)) return;

        // Ready 전환
        m_loadPlayer.SetState(Player.State.Move);
    }

    public override void OperateExit()
    {
        m_loadPlayer.ActionJoystick.AttackAction -= Attack;
        m_loadPlayer.ActionJoystick.AttackReadyAction -= GoToMove;

        m_loadPlayer.Animator.SetBool("NowReady", false);

        m_loadPlayer.rushUIEventSO.RaiseEvent(0); // hide
        m_loadPlayer.shootDirection.gameObject.SetActive(false);
    }

    public override void OperateUpdate()
    {
        m_loadPlayer.MoveComponent.Move(m_loadPlayer.MoveVec, m_loadPlayer.ActionVec, m_loadPlayer.Speed.IntervalValue * m_loadPlayer.ReadySpeedDecreaseRatio.IntervalValue, true);
        m_loadPlayer.MoveComponent.RotationPlayer(m_loadPlayer.ActionVec, true);

        float angle = Mathf.Atan2(-m_loadPlayer.ActionVec.y, -m_loadPlayer.ActionVec.x) * Mathf.Rad2Deg;

        m_loadPlayer.shootDirection.rotation = Quaternion.Euler(0, 0, angle);

        m_loadPlayer.RestoreRushRatio();
        m_loadPlayer.rushUIEventSO.RaiseEvent(m_loadPlayer.RushRatio); // hide
    }
}