using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttack : IState<Player.State>
{
    Player m_loadPlayer;

    public StatePlayerAttack(Player player)
    {
        m_loadPlayer = player;
    }

    public void OnAwakeMessage(Telegram<Player.State> telegram)
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
            m_loadPlayer.BattleComponent.UseSkill(SkillUseConditionType.Contact);
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

    public void OnProcessingMessage(Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    Vector2 savedAttackVec;
    Vector2 savedMoveVec;

    public void OperateEnter()
    {
        m_loadPlayer.Animator.SetBool("NowAttack", true);
        m_loadPlayer.DashComponent.PlayDash(savedAttackVec, m_loadPlayer.Data.RushThrust * m_loadPlayer.Data.RushRatio, m_loadPlayer.Data.RushTime);

        savedMoveVec = m_loadPlayer.MoveVec;

        m_loadPlayer.MoveComponent.RotateUsingTransform(savedAttackVec); // ���󰡴� �������� ��ȯ

        m_loadPlayer.ContactAction += ContactAct;
    }

    public void OperateExit()
    {
        m_loadPlayer.DashComponent.QuickEndTask(); // ���ǿ��� Ż���� ��, �ѹ� ��������
        m_loadPlayer.Animator.SetBool("NowAttack", false);
        m_loadPlayer.ContactAction -= ContactAct;
    }

    public void OperateUpdate()
    {
        CheckSwitchStates();
    }

    public bool CheckOverMinValue(Vector2 savedMoveVec, Vector2 dir, float attackCancelOffset)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(savedMoveVec.x - dir.x), 2) + Mathf.Pow(Mathf.Abs(savedMoveVec.y - dir.y), 2));

        if (distance > attackCancelOffset) return false;// ������ ���
        else return true;
    }

    public void CheckSwitchStates()
    {
        if (m_loadPlayer.DashComponent.NowFinish == true || CheckOverMinValue(savedMoveVec, m_loadPlayer.MoveVec, m_loadPlayer.Data.AttackCancelOffset) == false)
        {
            m_loadPlayer.DashComponent.QuickEndTask();
            m_loadPlayer.SetState(Player.State.Move);
        }
    }

    public void OnSetToGlobalState()
    {
    }
}