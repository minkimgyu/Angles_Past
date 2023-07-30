using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StateYellowHexaagonAttack : StateFollowEnemyAttack
{
    YellowHexagonEnemy loadYellowHexagonEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    public StateYellowHexaagonAttack(YellowHexagonEnemy yellowHexagonEnemy) : base(yellowHexagonEnemy)
    {
        loadYellowHexagonEnemy = yellowHexagonEnemy;
    }

    // 계속 발사해야해서 unitask로 제작      
    public override void ExecuteInRangeMethod()
    {
        attackFlag = true;
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        if(attackFlag)
        {
            if(canAttack == true)
            {
                // state를 스킬 사용 시 --> 정지 --> 추적으로 바꿔줌
                loadYellowHexagonEnemy.BattleComponent.UseSkill(SkillUseConditionType.InRange);
                loadYellowHexagonEnemy.SetState(BaseFollowEnemy.State.Fix);
                canAttack = false;
            }
        }

        if(canAttack == false)
        {
            storedTime += Time.deltaTime;
            if(storedTime > loadYellowHexagonEnemy.Delay)
            {
                storedTime = 0;
                canAttack = true;
            }
        }
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false;
    }
}

public class StateYellowHexaagonFix : IState<BaseFollowEnemy.State>
{
    YellowHexagonEnemy loadYellowHexagonEnemy;

    float storedTime;
    bool canRevertToPreviousState = false;

    public StateYellowHexaagonFix(YellowHexagonEnemy yellowHexagonEnemy)
    {
        loadYellowHexagonEnemy = yellowHexagonEnemy;
    }

    public void CheckSwitchStates()
    {
    }

    public void OnAwakeMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public void OnProcessingMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        canRevertToPreviousState = false;
        loadYellowHexagonEnemy.MoveComponent.Stop();
    }

    public void OperateExit()
    {
        
    }

    public void OperateUpdate()
    {
        loadYellowHexagonEnemy.Rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        if (canRevertToPreviousState == false)
        {
            storedTime += Time.deltaTime;
            if (storedTime > loadYellowHexagonEnemy.FixTime)
            {
                storedTime = 0;
                canRevertToPreviousState = true;
            }
        }

        if (canRevertToPreviousState)
        {
            loadYellowHexagonEnemy.Rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            loadYellowHexagonEnemy.RevertToPreviousState();
        } 
    }
}
