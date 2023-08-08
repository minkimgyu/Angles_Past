using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseReflectEnemy : Enemy<BaseReflectEnemy.State>
{
    ContactComponent m_contactComponent;
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    ReflectComponent m_reflectComponent;
    public ReflectComponent ReflectComponent { get { return m_reflectComponent; } }

    [SerializeField]
    Vector2 rushVec; // 랜덤으로 초기화
    public Vector2 RushVec { get { return rushVec; } set { rushVec = value; } }

    [SerializeField]
    float minRushVec; // 랜덤으로 초기화
    public float MinRushVec { get { return minRushVec; } set { minRushVec = value; } }

    public enum State
    {
        Rush,
        Reflect,
        Damaged
    }

    protected override void Awake()
    {
        base.Awake();
        m_contactComponent = GetComponent<ContactComponent>();
        m_reflectComponent = GetComponent<ReflectComponent>();

        rushVec = ResetRushVec();
    }

    public Vector2 ResetRushVec()
    {
        Vector2 tmpVec = Vector2.zero;

        while (tmpVec == Vector2.zero)
        {
           tmpVec = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }

        return tmpVec;
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        BaseState<State> rush = new StateReflectEnemyRush(this);
        BaseState<State> reflect = new StateReflectEnemyReflect(this);
        BaseState<State> damaged = new StateReflectEnemyDamaged(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Rush, rush);
        m_dicState.Add(State.Reflect, reflect);
        m_dicState.Add(State.Damaged, damaged);

        SetUp(State.Rush);
    }
}
