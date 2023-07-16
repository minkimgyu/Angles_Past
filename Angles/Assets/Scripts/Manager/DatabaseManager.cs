using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ActionMode { Idle, AttackReady, Attack, Dash, Follow, Hit }; // 동작 상태 모음

//public enum SkillName { None, NormalKnockBack, KnockBack, RotationBall, BigImpact, Blade, StickyBomb, GravitationalField, 
//    SelfDestruction, ShootBullet, ShockWave }; // 동작 상태 모음

public enum SkillUseType { Contact, Get }

public enum SkillUseCount { Uncountable, Countable } // --> 사용 시, 사용 가능 횟수를 1 빼거나 사용 횟수 차감없이 고정시킴
// 스킬 사용 시, 횟수 차감 여부
public enum SkillSynthesis { CountUp, NoCount } // --> 사용 시, 사용 가능 횟수를 1 빼거나 사용 횟수 차감없이 고정시킴
// 스킬 획득 시, 횟수 증감 여부
public enum EntityTag { Player, Enemy, Bullet, InnerSprite, Wall};

//[System.Serializable]
//public class DamageMethod
//{
//    public float damage;
//    public List<EntityTag> enemyTags;

//    public bool CheckTags(string tag)
//    {
//        for (int i = 0; i < enemyTags.Count; i++)
//        {
//            if (tag == enemyTags[i].ToString()) return true;
//        }

//        return false;
//    }
//}

public class DatabaseManager : Singleton<DatabaseManager>
{
    [SerializeField]
    PlayerData playerData;
    public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }

    //public List<ScriptableSkillData> m_scriptableSkillDatas; // 스킬 데이터 모음

    [SerializeField]
    List<BaseBuff> baseBuffs;
    public List<BaseBuff> BaseBuffs { get { return baseBuffs; }}

    [SerializeField]
    List<BaseSkill> baseSkills;
    public List<BaseSkill> BaseSkills { get { return baseSkills; } }

    [SerializeField]
    SkillCallData skillCallData;
    public SkillCallData SkillCallData { get { return skillCallData; } }

    protected override void Awake()
    {
        base.Awake();
    }

    public SkillData ReturnSkillData(string name)
    {
        return EntityDB.Skill.Find(x => x.Name == name).CopyData();
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