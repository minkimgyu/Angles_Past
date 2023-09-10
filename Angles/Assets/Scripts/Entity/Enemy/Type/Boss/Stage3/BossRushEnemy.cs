using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRushEnemy : DelayFollowEnemy
{
    BuffFloat attackThrust;
    public BuffFloat AttackThrust { get { return attackThrust; } }

    BuffFloat attackDuration;
    public BuffFloat AttackDuration { get { return attackDuration; } }

    ContactComponent contactComponent;
    public ContactComponent ContactComponent { get { return contactComponent; } }

    public Transform rushDirectionIcon;

    public void InvokeDisableRushDirectionIcon()
    {
        CancelInvoke();

        Vector3 tmpDir = (LoadPlayer.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(tmpDir.y, tmpDir.x) * Mathf.Rad2Deg;
        rushDirectionIcon.rotation = Quaternion.Euler(0, 0, angle);

        rushDirectionIcon.gameObject.SetActive(true);

        Invoke("DisableRushIcon", 1.5f);
    }

    void DisableRushIcon()
    {
        rushDirectionIcon.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out contactComponent);
    }

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffInt goldCount, BuffFloat spawnPercentage, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance, BuffFloat followDistance,
        BuffFloat followOffsetDistance, BuffFloat attackDelay, BuffFloat attackThrust, BuffFloat attackDuration)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance, attackDelay);

        this.attackThrust = attackThrust;
        this.attackDuration = attackDuration;
    }

    protected override void AddState()
    {
        BaseState<State> global = new StateBossRushEnemyGlobal(this); // 공격만 따로 추가해주자
        BaseState<State> attack = new StateBossRushEnemyAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Global, global);
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Global, global);
    }
}
