using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class StateYellowOctagonAttack : StateFollowEnemyAttack
{
    YellowOctagonEnemy loadYellowOctagonEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    List<int> angles = new List<int>();

    public StateYellowOctagonAttack(YellowOctagonEnemy yellowHexagonEnemy) : base(yellowHexagonEnemy)
    {
        loadYellowOctagonEnemy = yellowHexagonEnemy;
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
                // 첫번째 레이저 스킬
                loadYellowOctagonEnemy.BattleComponent.PossessingSkills[0].Directions = ReturnRandomDirection();

                ResetExpectDirection();

                // state를 스킬 사용 시 --> 정지 --> 추적으로 바꿔줌
                loadYellowOctagonEnemy.BattleComponent.UseSkill(SkillUseConditionType.InRange);
                canAttack = false;
            }
        }

        if(canAttack == false)
        {
            storedTime += Time.deltaTime;
            if(storedTime > loadYellowOctagonEnemy.Delay)
            {
                OffDirection();
                storedTime = 0;
                canAttack = true;
            }
        }
    }

    void ResetExpectDirection()
    {
        for (int i = 0; i < loadYellowOctagonEnemy.ExpectationDirs.Length; i++)
        {
            loadYellowOctagonEnemy.ExpectationDirs[i].SetActive(true);

            Vector3 offset = Vector3.right * 3.5f;
            Quaternion rotation = Quaternion.Euler(0, 0, angles[i]);
            Vector3 rotatedOffset = rotation * offset;

            Quaternion rotation1 = Quaternion.Euler(0, 0, angles[i] - 90);

            loadYellowOctagonEnemy.ExpectationDirs[i].transform.localPosition = rotatedOffset;
            loadYellowOctagonEnemy.ExpectationDirs[i].transform.localRotation = rotation1;
        }
    }

    void OffDirection()
    {
        for (int i = 0; i < loadYellowOctagonEnemy.ExpectationDirs.Length; i++)
        {
            loadYellowOctagonEnemy.ExpectationDirs[i].SetActive(false);
        }
    }

    List<Vector3> ReturnRandomDirection()
    {
        List<Vector3> directions = new List<Vector3>();
        angles.Clear();

        for (int i = 0; i < loadYellowOctagonEnemy.DirectionCount; i++)
        {
            int angle = Random.Range(0, 361);
            angles.Add(angle);

            directions.Add(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0));
        }

        return directions;
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false;
    }
}