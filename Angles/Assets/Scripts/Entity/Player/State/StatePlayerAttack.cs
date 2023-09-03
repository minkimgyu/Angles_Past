using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttack : BaseState<Player.State>
{
    Dictionary<string, System.Action<Collision2D>> CollisionTask;

    Player m_loadPlayer;

    Vector2 savedAttackVec;
    Vector2 savedMoveVec;

    public StatePlayerAttack(Player player)
    {
        m_loadPlayer = player;

        CollisionTask = new Dictionary<string, System.Action<Collision2D>>()
        {
            { "Wall", WhenContactWithWall },
            { "Enemy", WhenContactToEnemy }
        };
    }

    public override void OnMessage(Telegram<Player.State> telegram)
    {
        if (telegram.SenderStateName == Player.State.AttackReady || telegram.SenderStateName == Player.State.Reflect)
        {
            savedAttackVec = telegram.Message.dir;
        }
    }

    void GoToReflectState(Collision2D collision)
    {
        Message<Player.State> message = new Message<Player.State>();
        message.dir = m_loadPlayer.ReflectComponent.ResetReflectVec(collision.contacts[0].normal);
        Telegram<Player.State> telegram = new Telegram<Player.State>(Player.State.Attack, Player.State.Reflect, message);

        m_loadPlayer.SetState(Player.State.Reflect, telegram);
    }

    void WhenContactWithWall(Collision2D collision) => GoToReflectState(collision);

    void WhenContactToEnemy(Collision2D collision)
    {
        collision.gameObject.TryGetComponent(out IAvatar avatar);
        if (avatar == null) return;

        m_loadPlayer.SkillController.UseSkill(BaseSkill.UseConditionType.Contact);

        if (avatar.Weight.IntervalValue > avatar.Weight.IntervalValue)
        {
            if (collision != null && collision.contacts.Length != 0)
            {
                GoToReflectState(collision);
            }
        }
    }

    public override void ReceiveCollisionEnter(Collision2D collision) 
    {
        CollisionTask[collision.transform.tag](collision);
    }

    public void OnProcessingMessage(Telegram<Player.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        m_loadPlayer.Animator.SetBool("NowAttack", true);
        m_loadPlayer.DashComponent.PlayDash(savedAttackVec, m_loadPlayer.RushThrust.IntervalValue * m_loadPlayer.RushRatio, m_loadPlayer.RushDuration.IntervalValue);

        savedMoveVec = m_loadPlayer.MoveVec;

        m_loadPlayer.MoveComponent.RotateUsingTransform(savedAttackVec); // 날라가는 방향으로 전환
    }

    public override void OperateExit()
    {
        m_loadPlayer.DashComponent.QuickEndTask(); // 조건에서 탈출할 때, 한번 리셋해줌
        m_loadPlayer.Animator.SetBool("NowAttack", false);
    }

    public override void OperateUpdate()
    {
        CheckSwitchStates();
    }

    public bool CheckOverMinValue(Vector2 savedMoveVec, Vector2 dir, float attackCancelOffset)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(savedMoveVec.x - dir.x), 2) + Mathf.Pow(Mathf.Abs(savedMoveVec.y - dir.y), 2));

        if (distance > attackCancelOffset) return false;// 공격인 경우
        else return true;
    }

    public override void CheckSwitchStates()
    {
        if (m_loadPlayer.DashComponent.NowFinish == true || CheckOverMinValue(savedMoveVec, m_loadPlayer.MoveVec, m_loadPlayer.AttackCancelOffset) == false)
        {
            m_loadPlayer.DashComponent.QuickEndTask();
            m_loadPlayer.SetState(Player.State.Move);
        }
    }
}



//void ContactAct(Collision2D col) // 스킬 사용 + Reflect 채크해서 함수 사용
//{
//    col.gameObject.TryGetComponent(out Entity entity);
//    if (entity == null) return;


//    if (entity.InheritedTag == EntityTag.Enemy)
//    {
//        m_loadPlayer.BattleComponent.UseSkill(SkillUseConditionType.Contact);



//        col.gameObject.TryGetComponent(out BaseFollowEnemy followEnemy);
//        col.gameObject.TryGetComponent(out BaseReflectEnemy reflectEnemy);

//        col.gameObject.TryGetComponent(out Enemy followEnemy);


//        if (followEnemy != null)
//        {
//            if(followEnemy.HealthData.Weight > m_loadPlayer.HealthData.Weight)
//            {
//                if (col != null && col.contacts.Length != 0)
//                {
//                    Message<PlayerTransform.State> message = new Message<PlayerTransform.State>();
//                    message.dir = m_loadPlayer.ReflectComponent.ResetReflectVec(col.contacts[0].normal);
//                    Telegram<PlayerTransform.State> telegram = new Telegram<PlayerTransform.State>(PlayerTransform.State.Attack, PlayerTransform.State.Reflect, message);

//                    m_loadPlayer.SetState(PlayerTransform.State.Reflect, telegram);
//                }
//            }
//        }
//        else if(reflectEnemy != null)
//        {
//            if (reflectEnemy.HealthData.Weight > m_loadPlayer.HealthData.Weight)
//            {
//                if (col != null && col.contacts.Length != 0)
//                {
//                    Message<PlayerTransform.State> message = new Message<PlayerTransform.State>();
//                    message.dir = m_loadPlayer.ReflectComponent.ResetReflectVec(col.contacts[0].normal);
//                    Telegram<PlayerTransform.State> telegram = new Telegram<PlayerTransform.State>(PlayerTransform.State.Attack, PlayerTransform.State.Reflect, message);

//                    m_loadPlayer.SetState(PlayerTransform.State.Reflect, telegram);
//                }
//            }
//        }


//        // 스킬 사용
//    }

//    if (entity.InheritedTag == EntityTag.Wall)
//    {
//        Message<PlayerTransform.State> message = new Message<PlayerTransform.State>();
//        message.dir = m_loadPlayer.ReflectComponent.ResetReflectVec(col.contacts[0].normal);
//        Telegram<PlayerTransform.State> telegram = new Telegram<PlayerTransform.State>(PlayerTransform.State.Attack, PlayerTransform.State.Reflect, message);

//        m_loadPlayer.SetState(PlayerTransform.State.Reflect, telegram);
//    }
//}