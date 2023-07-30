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

    // ��� �߻��ؾ��ؼ� unitask�� ����      
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
                // ù��° ������ ��ų
                loadYellowOctagonEnemy.BattleComponent.PossessingSkills[0].Directions = ReturnRandomDirection();

                ResetExpectDirection();

                // state�� ��ų ��� �� --> ���� --> �������� �ٲ���
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