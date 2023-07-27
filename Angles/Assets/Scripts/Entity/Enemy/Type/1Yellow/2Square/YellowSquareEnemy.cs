using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSquareEnemy : BaseFollowEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateYellowSquareAttack(this); // ���ݸ� ���� �߰�������
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);

        if (BattleComponent.PossessingSkills == null || BattleComponent.PossessingSkills.Count == 0) return;

        float delayTime = BattleComponent.PossessingSkills[0].PreDelay; // ù��° ��ų�� PreDelay�� �ִ´�.
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
