using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SkillType { KnockBack }
public enum ActionMode { Idle, AttackReady, Attack, Dash, Follow, Hit }; // 동작 상태 모음

public enum SkillMode { KnockBack, }; // 동작 상태 모음

public enum EntityTag { Player, Enemy, Wall };

public class DatabaseManager : Singleton<DatabaseManager>
{
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
    float attackDamage = 5;
    public float AttackDamage { get { return attackDamage; } set { attackDamage = value; } }

    #endregion

    [Header("EntityDB")]
    [SerializeField]
    EntityDB entityDB;
    public EntityDB EntityDB { get { return entityDB; } set { entityDB = value; } }

    public bool CanUseDash()
    {
        if(DashRatio - 1 / MaxDashCount >= 0)
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