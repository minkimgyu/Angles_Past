using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionMode { Idle, AttackReady, Attack, Dash, Follow, Hit }; // 동작 상태 모음

public enum SkillName { None, NormalKnockBack, KnockBack, RotationBall, BigImpact, Blade, StickyBomb, GravitationalField, 
    SelfDestruction, ShootBullet, ShockWave }; // 동작 상태 모음

public enum SkillUseType { None, Contact, Get, Start, Condition }

public enum EntityTag { Player, Enemy, Bullet };

[System.Serializable]
public class DamageMethod
{
    public float damage;
    public List<EntityTag> enemyTags;

    public bool CheckTags(string tag)
    {
        for (int i = 0; i < enemyTags.Count; i++)
        {
            if (tag == enemyTags[i].ToString()) return true;
        }

        return false;
    }
}

public class DatabaseManager : Singleton<DatabaseManager>
{
    [SerializeField]
    PlayerData playerData;
    public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }

    protected override void Awake()
    {
        base.Awake();
    }

    public SkillData ReturnSkillData(SkillName skillName)
    {
        return EntityDB.Skill.Find(x => x.Name == skillName).CopyData();
    }

    public EnemyData ReturnEnemyData(string name)
    {
        return EntityDB.Enemy.Find(x => x.Name == name).CopyData();
    }

    [Header("EntityDB")]
    [SerializeField]
    EntityDB entityDB;
    public EntityDB EntityDB { get { return entityDB; }}
}