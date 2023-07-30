using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BaseFollowEnemy
{
    [SerializeField]
    float delay;
    public float Delay { get { return delay; } }

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateBossEnemyAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
