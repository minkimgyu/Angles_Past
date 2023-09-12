using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

// DirectionCount 이 부분을 Interfact를 사용해서 뜯어보자
public interface ISpecifyDirection
{
    public Vector3[] Directions { get; set; } // 이걸 받아와서 스킬 사용
    public void ResetRandomDirection(out List<int> angles); // angles는 각도

    public void ActiveExpectDirectionPoint(List<int> angles); // 예상 이미지 위치 지정

    public void OffExpectDirectionPoint(); // 예상 위치 숨기기
}


public class YellowOctagonEnemy : DelayFollowEnemy, ISpecifyDirection
{
    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
        BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
        BuffInt score, BuffInt goldCount, BuffFloat spawnPercentage, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance,
        BuffFloat followDistance, BuffFloat followOffsetDistance, BuffFloat attackDelay, BuffInt directionCount)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance, attackDelay);

        this.directionCount = directionCount.CopyData();
    }

    [SerializeField]
    GameObject[] expectationDirs;
    public GameObject[] ExpectationDirs { get { return expectationDirs; } }

    [SerializeField]
    BuffInt directionCount; // 개수만 정해줌
    public BuffInt DirectionCount { get { return directionCount; } }

    [HideInInspector]
    public Vector3[] Directions { get; set; }

    [HideInInspector]
    public float distanceFromCenter = 3.5f;
    

    protected override void AddState()
    {
        BaseState<State> attack = new StateYellowOctagonAttack(this); // 공격만 따로 추가해주자
        m_dicState.Add(State.Attack, attack);

        AddBaseState();

        SetUp(State.Follow);
        SetGlobalState(State.Attack, attack);
    }

    public void ResetRandomDirection(out List<int> angles)
    {
        List<Vector3> directions = new List<Vector3>();
        List<int> directionAngles = new List<int>(); // 방향 각도

        for (int i = 0; i < directionCount.IntervalValue; i++)
        {
            int angle = Random.Range(0, 361);
            directionAngles.Add(angle);
            directions.Add(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0));
        }

        angles = directionAngles;
        Directions = directions.ToArray();
    }

    public void ActiveExpectDirectionPoint(List<int> angles)
    {
        for (int i = 0; i < expectationDirs.Length; i++)
        {
            expectationDirs[i].SetActive(true);

            Vector3 offset = Vector3.right * distanceFromCenter;
            Quaternion rotation = Quaternion.Euler(0, 0, angles[i]);
            Vector3 rotatedOffset = rotation * offset;

            expectationDirs[i].transform.localPosition = rotatedOffset;
            expectationDirs[i].transform.localRotation = rotation;
        }
    }

    public void OffExpectDirectionPoint()
    {
        for (int i = 0; i < expectationDirs.Length; i++)
        {
            expectationDirs[i].SetActive(false);
        }
    }
}
