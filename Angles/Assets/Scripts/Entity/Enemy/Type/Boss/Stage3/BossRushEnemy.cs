using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRushEnemy : DelayFollowEnemy
{
    BuffFloat attackThrust;
    public BuffFloat AttackThrust { get { return attackThrust; } }

    ContactComponent contactComponent;
    public ContactComponent ContactComponent { get { return contactComponent; } }

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out contactComponent);
    }

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance, BuffFloat followDistance,
        BuffFloat followOffsetDistance, BuffFloat attackDelay, BuffFloat attackThrust)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance, attackDelay);

        this.attackThrust = attackThrust;
    }

    protected override void AddState()
    {
        BaseState<State> attack = new StateBossRushEnemyAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }
}
