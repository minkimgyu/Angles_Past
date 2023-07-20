using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPentagonEnemy : BaseFollowEnemy
{
    [SerializeField]
    float delay;
    public float Delay { get { return delay; } }

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateYellowPentagonAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack);

        SetUp(State.Follow);
        SetGlobalState(attack);
    }
}
