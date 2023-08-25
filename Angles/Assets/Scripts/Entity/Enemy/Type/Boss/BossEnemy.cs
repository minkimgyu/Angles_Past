using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : DelayFollowEnemy
{
    protected override void AddState()
    {
        BaseState<State> attack = new StateBossEnemyAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(attack);
    }

    //SO�� ȣ��

    //public override void Die()
    //{
    //    base.Die();
    //    PlayManager.instance.GameClear();
    //}
}
