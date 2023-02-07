using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionMode { Idle, AttackReady }; // 기본 상태와 공격준비 상태

public enum PlayerMode { Idle, Attack, Dash }; // 기본, 공격, 대쉬 상태

public class DatabaseManager : Singleton<DatabaseManager>
{
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
    float followSpeed = 5;
    public float FollowSpeed { get { return followSpeed; } set { followSpeed = value; } }

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