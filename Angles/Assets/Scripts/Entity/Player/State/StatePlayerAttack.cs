using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttack : IState<Player, Player.State>
{
    Player m_loadPlayer;

    public StatePlayerAttack(Player player)
    {
        m_loadPlayer = player;
    }

    public void OnAwakeMessage(Player value, Telegram<Player.State> telegram)
    {
        if (telegram.SenderStateName == Player.State.AttackReady || telegram.SenderStateName == Player.State.Reflect)
        {
            savedAttackVec = telegram.Message.dir;
        }
    }


    void ContactAct(Collision2D col) // ��ų ��� + Reflect äũ�ؼ� �Լ� ���
    {
        col.gameObject.TryGetComponent(out Entity entity);
        if (entity == null) return;


        if (entity.InheritedTag == EntityTag.Enemy)
        {
            m_loadPlayer.BattleComponent.UseSkill(SkillUseType.Contact);
            // ��ų ���
        }

        if (entity.InheritedTag == EntityTag.Wall)
        {
            Message<Player.State> message = new Message<Player.State>();
            message.dir = m_loadPlayer.ReflectComponent.ResetReflectVec(col.contacts[0].normal);
            Telegram<Player.State> telegram = new Telegram<Player.State>(Player.State.Attack, Player.State.Reflect, message);

            m_loadPlayer.SetState(Player.State.Reflect, telegram);
        }
    }

    public void OnProcessingMessage(Player value, Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    Vector2 savedAttackVec;
    Vector2 savedMoveVec;

    public void OperateEnter(Player player)
    {
        player.Animator.SetBool("NowAttack", true);
        player.DashComponent.PlayDash(savedAttackVec, player.Data.RushThrust * player.Data.RushRatio, player.Data.RushTime);

        savedMoveVec = player.MoveVec;

        player.MoveComponent.RotateUsingTransform(savedAttackVec); // ���󰡴� �������� ��ȯ

        player.ContactAction += ContactAct;
    }

    public void OperateExit(Player player)
    {
        player.DashComponent.QuickEndTask(); // ���ǿ��� Ż���� ��, �ѹ� ��������
        player.Animator.SetBool("NowAttack", false);
        player.ContactAction -= ContactAct;
    }

    public void OperateUpdate(Player player)
    {
        CheckSwitchStates(player);
    }

    public bool CheckOverMinValue(Vector2 savedMoveVec, Vector2 dir, float attackCancelOffset)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(savedMoveVec.x - dir.x), 2) + Mathf.Pow(Mathf.Abs(savedMoveVec.y - dir.y), 2));

        if (distance > attackCancelOffset) return false;// ������ ���
        else return true;
    }

    public void CheckSwitchStates(Player player)
    {
        if (player.DashComponent.NowFinish == true || CheckOverMinValue(savedMoveVec, player.MoveVec, player.Data.AttackCancelOffset) == false)
        {
            player.DashComponent.QuickEndTask();
            player.SetState(Player.State.Move);
        }
    }
}