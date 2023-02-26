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


    #region 변수 모음

    [Header("Attack")]
    [SerializeField]
    int attackThrust = 5;
    public int AttackThrust { get { return attackThrust; } set { attackThrust = value; } }

    [SerializeField]
    int reflectAttackThrust = 2;
    public int ReflectAttackThrust { get { return reflectAttackThrust; } set { reflectAttackThrust = value; } }

    [SerializeField]
    float attackTime = 3f;
    public float AttackTime { get { return attackTime; } set { attackTime = value; } }

    [SerializeField]
    float attackCancelOffset = 0.2f;
    public float AttackCancelOffset { get { return attackCancelOffset; } set { attackCancelOffset = value; } }

    [Header("Dash")]
    [SerializeField]
    float maxDashCount = 3;
    public float MaxDashCount { get { return maxDashCount; } set { maxDashCount = value; } }

    [SerializeField]
    float dashTime = 0.5f;
    public float DashTime { get { return dashTime; } set { dashTime = value; } }

    [SerializeField]
    int dashThrust = 8;
    public int DashThrust { get { return dashThrust; } set { dashThrust = value; } }

    [SerializeField]
    float dashRatio = 1;
    public float DashRatio
    {
        get
        {
            return dashRatio;
        } 
        set 
        {
            dashRatio = value;

            if (dashRatio > 1f)
            {
                dashRatio = 1f;
                return;
            }
        } 
    }

    [Header("Move")]
    [SerializeField]
    float moveSpeed = 5;
    public float MoveSpeed{ get{ return moveSpeed; } set { moveSpeed = value; } }

    [SerializeField]
    float readySpeed = 2; // 움직임
    public float ReadySpeed { get { return readySpeed; } set { readySpeed = value; } }

    [SerializeField]
    float minSpeedRatio = 0.2f; // 움직임
    public float MinSpeedRatio { get { return minSpeedRatio; } }

    [SerializeField]
    float maxSpeedRatio = 1f; // 움직임
    public float MaxSpeedRatio { get { return maxSpeedRatio; } }

    [SerializeField]
    float speedRatio = 1; // 움직임
    public float SpeedRatio { get { return speedRatio; } set { speedRatio = value; if (MinSpeedRatio >= speedRatio) { speedRatio = MinSpeedRatio; } if (speedRatio >= MaxSpeedRatio) { speedRatio = maxSpeedRatio; } } }

    [Header("Follow")]
    [SerializeField]
    float followSpeed = 3;
    public float FollowSpeed { get { return followSpeed; } set { followSpeed = value; } }

    [SerializeField]
    float minFollowDistance = 5;
    public float MinFollowDistance { get { return minFollowDistance; } set { minFollowDistance = value; } }

    [SerializeField]
    float waitTime = 5;
    public float WaitTime { get { return waitTime; } set { waitTime = value; } }

    [Header("Damage")]
    [SerializeField]
    public EnumMethodDictionary damageDictionary;

    [Header("KnockBack")]
    [SerializeField]
    float thrustRatio = 1.2f;
    public float ThrustRatio { get { return thrustRatio; } set { thrustRatio = value; } }

   
    #endregion

    [Header("EntityDB")]
    [SerializeField]
    EntityDB entityDB;
    public EntityDB EntityDB { get { return entityDB; } set { entityDB = value; } }

    public bool CanUseDash()
    {
        if (DashRatio - 1 / MaxDashCount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SubtractRatio() 
    {
        DashRatio -= 1 / MaxDashCount;
    }
}