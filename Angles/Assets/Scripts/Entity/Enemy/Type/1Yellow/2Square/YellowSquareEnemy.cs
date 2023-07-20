using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSquareEnemy : BaseFollowEnemy
{
    ColorChangeComponent colorChangeComponent;
    public ColorChangeComponent ColorChangeComponent { get { return colorChangeComponent; } }

    protected override void Awake()
    {
        base.Awake();
        colorChangeComponent = GetComponent<ColorChangeComponent>();
    }

    protected override void Init()
    {
        base.Init();
        IState<State> attack = new StateYellowSquareAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack); 

        SetUp(State.Follow);
        SetGlobalState(attack);

        if (BattleComponent.PossessingSkills == null || BattleComponent.PossessingSkills.Count == 0) return;

        float delayTime = BattleComponent.PossessingSkills[0].PreDelay; // 첫번째 스킬의 PreDelay를 넣는다.
        colorChangeComponent.Init(delayTime);
    }

    protected override void OnDisable()
    {
        colorChangeComponent.ResetColor();
        base.OnDisable();
    }
}
