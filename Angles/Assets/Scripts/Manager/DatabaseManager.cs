using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ActionMode { Idle, AttackReady, Attack, Dash, Follow, Hit }; // ���� ���� ����

//public enum SkillName { None, NormalKnockBack, KnockBack, RotationBall, BigImpact, Blade, StickyBomb, GravitationalField, 
//    SelfDestruction, ShootBullet, ShockWave }; // ���� ���� ����

//public enum SkillOverlapType { None, Restart }

///// <summary>
///// InRange, OutRange�� ���� ���� �� �÷��̾ ���� ������ ���Դ��� �ƴ��� üũ��
///// </summary>
///// 
//public enum SkillUseConditionType { Contact, Get, InRange, OutRange, AddState, Rush }

//public enum SkillUseCountSubtractType { None, Subtract } // --> ��� ��, ��� ���� Ƚ���� 1 ���ų� ��� Ƚ�� �������� ������Ŵ
//// ��ų ��� ��, Ƚ�� ���� ����

//public enum SkillSynthesisType { None, CountUp } // --> ��� ��, ��� ���� Ƚ���� 1 ���ų� ��� Ƚ�� �������� ������Ŵ
//// ��ų ȹ�� ��, Ƚ�� ���� ����

public enum EntityTag { Player, Enemy, Bullet, InnerSprite, Wall, Construction, Spike};

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

//public class DatabaseManager : MonoBehaviour//Singleton<DatabaseManager>
//{
//    //[SerializeField]
//    //PlayerData playerData;
//    //public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }

//    //public List<ScriptableSkillData> m_scriptableSkillDatas; // ��ų ������ ����

//    public static DatabaseManager instance;
//    public static DatabaseManager Instance { get { return instance; } }

//    protected void Awake()
//    {
//        //base.Awake();
//        instance = this;
//    }

//    //public SkillData ReturnSkillData(string name)
//    //{
//    //    return EntityDB.Skill.Find(x => x.Name == name).CopyData();
//    //}

//    //public EnemyData ReturnEnemyData(string name)
//    //{
//    //    return EntityDB.Enemy.Find(x => x.Name == name).CopyData();
//    //}

//    [Header("EntityDB")]
//    [SerializeField]
//    EntityDB entityDB;
//    public EntityDB EntityDB { get { return entityDB; }}

//    [Header("UtilizationDB")]
//    [SerializeField]
//    UtilizationDB utilizationDB;
//    public UtilizationDB UtilizationDB { get { return utilizationDB; } }
//}