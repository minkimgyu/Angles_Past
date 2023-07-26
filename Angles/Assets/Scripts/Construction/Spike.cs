using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : BasicConstruction
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        IState<State> idle = new StateBasicConstructionIdle(this);
        IState<State> attack = new StateBasicConstructionAttack(this);

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        m_dicState.Add(State.Idle, idle);
        m_dicState.Add(State.Attack, attack);

        SetUp(State.Idle);
        grantedUtilization.LootSkillFromDB(BattleComponent);
    }
}
