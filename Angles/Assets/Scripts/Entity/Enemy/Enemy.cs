using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Enemy<T> : HealthEntity<T>
{
    BaseEnemyData baseEnemyData;

    string[] dropSkills = { "GhostItem", "BarrierItem", "BladeItem", "KnockbackItem", "ShockwaveItem", "SpawnBallItem", "SpawnGravityBallItem", "StickyBombItem" };

    MoveComponent m_moveComponent;
    public MoveComponent MoveComponent { get { return m_moveComponent; } }

    DashComponent m_dashComponent;
    public DashComponent DashComponent { get { return m_dashComponent; } }

    BattleComponent m_battleComponent;
    public BattleComponent BattleComponent { get { return m_battleComponent; } }

    protected override void Awake()
    {
        base.Awake();
        m_moveComponent = GetComponent<MoveComponent>();
        m_dashComponent = GetComponent<DashComponent>();
        m_battleComponent = GetComponent<BattleComponent>();

        baseEnemyData.GrantedUtilization.LootSkillFromDB(BattleComponent);
    }

    void SpawnRandomItem()
    {
        float percentage = UnityEngine.Random.Range(0.0f, 1.0f);
        if (percentage > baseEnemyData.SpawnPercentage) return;

        ObjectPooler.SpawnFromPool<DropSkill>(dropSkills[UnityEngine.Random.Range(0, dropSkills.Length)], transform.position);
    }

    public void AddEnemyContactKnockback()
    {
        SkillData skillData = DatabaseManager.Instance.UtilizationDB.ReturnSkillData("EnemyContactKnockback");
        m_battleComponent.LootingSkill(skillData);
    }

    public void RemoveEnemyContactKnockback()
    {
        SkillData skillData = DatabaseManager.Instance.UtilizationDB.ReturnSkillData("EnemyContactKnockback");
        m_battleComponent.RemoveSkillFromPossessingSkills(skillData); // 만약 안 쓰고 존재한다면 삭제해준다.
    }

    public override void Die()
    {
        SpawnRandomItem();
        base.Die();
    }
}