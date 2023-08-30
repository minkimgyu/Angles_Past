using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Player : Avatar<Player.State>
{
    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime, BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName,
        BuffFloat readySpeedDecreaseRatio, BuffFloat rushThrust, BuffFloat rushRecoverRatio, BuffFloat rushDuration, float attackCancelOffset,
        BuffInt dashCount, BuffFloat dashDuration, BuffFloat dashThrust, BuffFloat dashRecoverRatio, string[] skillNames)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames);

        m_readySpeedDecreaseRatio = readySpeedDecreaseRatio;
        m_rushThrust = rushThrust;
        m_rushRecoverRatio = rushRecoverRatio;
        m_rushDuration = rushDuration;

        m_attackCancelOffset = attackCancelOffset;

        m_dashCount = dashCount;
        m_dashDuration = dashDuration;
        m_dashThrust = dashThrust;
        m_dashRecoverRatio = dashRecoverRatio;
    }

    // 변수

    BuffFloat m_readySpeedDecreaseRatio;
    public BuffFloat ReadySpeedDecreaseRatio { get { return m_readySpeedDecreaseRatio; } set { m_readySpeedDecreaseRatio = value; } }

    BuffFloat m_rushThrust;
    public BuffFloat RushThrust { get { return m_rushThrust; } set { m_rushThrust = value; } }

    float m_rushRatio = 0; // 범위 0 ~ 1까지
    public float RushRatio { get { return m_rushRatio; }}

    BuffFloat m_rushRecoverRatio; // 회복 비율
    public BuffFloat RushRecoverRatio { get { return m_rushRecoverRatio; } set { m_rushRecoverRatio = value; } }

    BuffFloat m_rushDuration;
    public BuffFloat RushDuration { get { return m_rushDuration; } set { m_rushDuration = value; } }

    /// <summary>
    /// 조이스틱 움직임 값에 따라 AttackReady에서 Move 상태로 왔다갔다함
    /// </summary>
    float m_attackCancelOffset;
    public float AttackCancelOffset { get { return m_attackCancelOffset; }}

    BuffInt m_dashCount;
    public BuffInt DashCount { get { return m_dashCount; } set { m_dashCount = value; } }

    BuffFloat m_dashDuration;
    public BuffFloat DashDuration { get { return m_dashDuration; } set { m_dashDuration = value; } }

    BuffFloat m_dashThrust;
    public BuffFloat DashThrust { get { return m_dashThrust; } set { m_dashThrust = value; } }

    /// <summary>
    /// 대쉬 비율
    /// </summary>
    float m_dashRatio = 1;
    public float DashRatio { get { return m_dashRatio; } }

    /// <summary>
    /// 대쉬 회복량
    /// </summary>
    BuffFloat m_dashRecoverRatio;
    public BuffFloat DashRecoverRatio { get { return m_dashRecoverRatio; } set { m_dashRecoverRatio = value; } }

    //

    Animator m_animator;
    public Animator Animator { get { return m_animator; } }

    AttackComponent m_attackComponent;
    public AttackComponent AttackComponent { get { return m_attackComponent; } }

    ContactComponent m_contactComponent;
    public ContactComponent ContactComponent { get { return m_contactComponent; } }

    ReflectComponent m_reflectComponent;
    public ReflectComponent ReflectComponent { get { return m_reflectComponent; } }

    LootingItemComponent m_lootingItemComponent;
    public LootingItemComponent LootingItemComponent { get { return m_lootingItemComponent; } }



    BarrierComponent m_barrierComponent;
    public BarrierComponent BarrierComponent { get { return m_barrierComponent; } }


    ActionJoystick actionJoycstick;
    public ActionJoystick ActionJoystick { get { return actionJoycstick; } }

    MoveJoystick moveJoystick;
    public MoveJoystick MoveJoystick { get { return moveJoystick; } }

<<<<<<< Updated upstream
=======
    Rigidbody2D m_rigid;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    public Color originColor;
    public Color immortalityColor;

>>>>>>> Stashed changes
    // 조이스틱 프로퍼티
    public Vector2 ActionVec { get { return actionJoycstick.MainVec; } }

    public Vector2 MoveVec { get { return moveJoystick.ReturnMoveVec(); } }

    [SerializeField]
    public PlayerActionEventSO rushUIEventSO;

    public Transform directionPointer;

    [SerializeField]
<<<<<<< Updated upstream
    public PlayerActionEventSO dashUIEventSO;
=======
    PlayerData _data;
    public PlayerData Data { get { return _data; } }

    [SerializeField]
    AdditionalPlayerData _additionalData;
    public AdditionalPlayerData AdditionalData { get { return _additionalData; } set { _additionalData = value; } }

    public override void InitData()
    {
        _data = DatabaseManager.Instance.EntityDB.Player.CopyData();
        SaveManager.instance.ResetPlayerData(_data);

        scaleUp = (maxScale - transform.localScale.x) / canEatCount;
        massUp = (maxMass - m_rigid.mass) / canEatCount;
        weightUp = (maxWeight - _data.Weight) / canEatCount;

        Debug.Log(weightUp);
        rushThrustUp = (maxRushThrust - _data.RushThrust) / canEatCount;
        dashThrustUp = (maxDashThrust - _data.DashThrust) / canEatCount;
    }
>>>>>>> Stashed changes

    public enum State
    {      
        Dash,
        AttackReady,
        Attack,
        Move,
        Die,
        Reflect,
        Battle,
        Damaged
    }

    public enum ObserverType
    {
       ShowDashUI,

       ShowRushUI,
       HideRushUI,
    }

    //float minJoyVal = 0.25f;

    //스테이트들을 보관
    //private Dictionary<State, IState<Player, Telegram<State>>> m_dicState = new Dictionary<State, IState<Player, Telegram<State>>>();
    private List<IObserver<ObserverType, PlayerData>> m_observers = new List<IObserver<ObserverType, PlayerData>>();

    protected override void Awake()
    {
<<<<<<< Updated upstream
        base.Awake();

=======
        spriteRenderer = GetComponent<SpriteRenderer>();

        m_dashComponent = GetComponent<DashComponent>();
>>>>>>> Stashed changes
        m_attackComponent = GetComponent<AttackComponent>();
        m_reflectComponent = GetComponent<ReflectComponent>();
        m_animator = GetComponent<Animator>();

        m_lootingItemComponent = GetComponent<LootingItemComponent>();

        m_contactComponent = GetComponent<ContactComponent>();
        m_barrierComponent = GetComponent<BarrierComponent>();

        moveJoystick = GameObject.FindWithTag("MoveJoystick").GetComponent<MoveJoystick>();
        actionJoycstick = GameObject.FindWithTag("AttackJoystick").GetComponent<ActionJoystick>();
<<<<<<< Updated upstream
=======
        
        m_reflectComponent.AbleTags.Add(EntityTag.Wall);

        //m_battleComponent.AbleTags.Add(EntityTag.Enemy);


        //actionJoycstick.DashAction += Dash;
        //actionJoycstick.AttackReadyAction += AttackReady;
        //actionJoycstick.AttackAction += Attack;

        // action, move 입력 받는 부분 찾아서 넣기
        // 델리게이트 연결시켜주기
        // 데이터 값 넘겨서 UI 초기화 해주기
        // 데이터 여기서 저장하기

        
    }

    int tmpCount = 0;
    int canEatCount = 100;

    float maxScale = 0.3f;
    float maxMass = 5.0f;
    float maxWeight = 1.2f;
    float maxRushThrust = 150.0f;
    float maxDashThrust = 100.0f;

    float scaleUp = 0;
    float massUp = 0;
    float weightUp = 0;
    float rushThrustUp = 0;
    float dashThrustUp = 0;

    public void UpMassAndScale()
    {
        tmpCount++;
        if (canEatCount < tmpCount) return;

        transform.localScale += new Vector3(scaleUp, scaleUp, 0);
        m_rigid.mass += massUp;
        _data.Weight += weightUp;
        _data.RushThrust += rushThrustUp;
        _data.DashThrust += dashThrustUp;
>>>>>>> Stashed changes
    }

    private void Start()
    {
        //상태 생성
        BaseState<State> move = new StatePlayerMove(this);
        BaseState<State> attack = new StatePlayerAttack(this);
        BaseState<State> attackReady = new StatePlayerAttackReady(this);
        BaseState<State> dash = new StatePlayerDash(this);
        BaseState<State> dead = new StatePlayerDie(this);
        BaseState<State> reflect = new StatePlayerReflect(this);
        BaseState<State> damaged = new StatePlayerDamaged(this);

        BaseState<State> global = new StatePlayerGlobal(this);

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Move, move);
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.AttackReady, attackReady);
        m_dicState.Add(State.Dash, dash);
        m_dicState.Add(State.Die, dead);
        m_dicState.Add(State.Reflect, reflect);
        m_dicState.Add(State.Damaged, damaged);

        SetUp(State.Move);
        SetGlobalState(global);
    }

    
    public void ResetRushRatioToZero() => m_rushRatio = 0;

    public bool RestoreRatio(ref float ratio, float recoverRatio)
    {
        if (ratio < 1)
        {
            ratio += recoverRatio * Time.deltaTime;
            if (ratio > 1) ratio = 1;

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RestoreDashRatio() { return RestoreRatio(ref m_dashRatio, m_dashRecoverRatio.IntervalValue); }

    public bool RestoreRushRatio() { return RestoreRatio(ref m_rushRatio, m_rushRecoverRatio.IntervalValue); }

    public void SubtractDashRatio() => m_dashRatio -= 1 / m_dashCount.IntervalValue;

    public bool CheckOverMinValue(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > AttackCancelOffset || Mathf.Abs(dir.y) > AttackCancelOffset) return true;// 공격인 경우
        else return false;
    }
}
