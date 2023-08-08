using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Player : HealthEntity<Player.State>, ISubject<Player.ObserverType, PlayerData>
{
    Animator m_animator;
    public Animator Animator { get { return m_animator; } }

    AttackComponent m_attackComponent;
    public AttackComponent AttackComponent { get { return m_attackComponent; } }

    DashComponent m_dashComponent;
    public DashComponent DashComponent { get { return m_dashComponent; } }

    MoveComponent m_moveComponent;
    public MoveComponent MoveComponent { get { return m_moveComponent; } }

    BattleComponent m_battleComponent;
    public BattleComponent BattleComponent { get { return m_battleComponent; } }

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

    // 조이스틱 프로퍼티
    public Vector2 ActionVec { get { return actionJoycstick.MainVec; } }

    public Vector2 MoveVec { get { return moveJoystick.ReturnMoveVec(); } }


    //public Action<Collision2D> ContactAction;

    //public Action<float, Vector2, float> UnderAttackAction;

    [SerializeField]
    PlayerData m_playerData;
    public PlayerData PlayerData { get { return m_playerData; } }

    public override void InitData()
    {
        PlayerStat playerStat = DatabaseManager.Instance.EntityDB.PlayerStat;

        HealthData = playerStat.HealthData;
        m_playerData = playerStat.PlayerData;
    }

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
        base.Awake();

        m_dashComponent = GetComponent<DashComponent>();
        m_attackComponent = GetComponent<AttackComponent>();
        m_moveComponent = GetComponent<MoveComponent>();
        m_reflectComponent = GetComponent<ReflectComponent>();
        m_battleComponent = GetComponent<BattleComponent>();
        m_animator = GetComponent<Animator>();

        m_lootingItemComponent = GetComponent<LootingItemComponent>();

        m_contactComponent = GetComponent<ContactComponent>();
        m_barrierComponent = GetComponent<BarrierComponent>();

        moveJoystick = GameObject.FindWithTag("MoveJoystick").GetComponent<MoveJoystick>();
        actionJoycstick = GameObject.FindWithTag("AttackJoystick").GetComponent<ActionJoystick>();
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

        PlayerData.GrantedUtilization.LootSkillFromDB(BattleComponent);

        SetUp(State.Move);
        SetGlobalState(global);
    }

    public bool CheckOverMinValue(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > m_playerData.AttackCancelOffset || Mathf.Abs(dir.y) > m_playerData.AttackCancelOffset) return true;// 공격인 경우
        else return false;
    }

    public void AddObserver(IObserver<ObserverType, PlayerData> observer)
    {
        m_observers.Add(observer);
    }

    public void RemoveObserver(IObserver<ObserverType, PlayerData> observer)
    {
        m_observers.Remove(observer);
    }

    public void NotifyObservers(ObserverType state, PlayerData data)
    {
        m_observers.ForEach((m_observers) =>
        {
            m_observers.OnNotify(state, data);
        });
    }

    //public EntityTag ReturnTag()
    //{
    //    return inheritedTag;
    //}

    //public void UnderAttack(float healthPoint, Vector2 dir, float thrust)
    //{
    //    if(PlayManager.Instance.GameClearCheck == true) return;

    //    SoundManager.Instance.PlaySFX(transform.position, "Hit", 0.7f);
    //    //if (UnderAttackAction != null) UnderAttackAction(healthPoint, dir, thrust);
    //}

    //public void DestoryThis()
    //{
    //    gameObject.SetActive(false);
    //    //Destroy(gameObject);
    //}

    //public override void Die()
    //{
    //    base.Die();
    //}

    //public override HealthEntityData ReturnHealthEntityData()
    //{
    //    return Data;
    //}
}
