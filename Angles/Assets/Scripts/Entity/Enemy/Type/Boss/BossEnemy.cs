using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : DelayFollowEnemy
{
    protected override void AddState()
    {
        base.AddState();
        BaseState<State> attack = new StateBossEnemyAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(attack);
    }

    //public override void Die()
    //{
    //    base.Die();
    //    PlayManager.instance.GameClear();
    //}
}
