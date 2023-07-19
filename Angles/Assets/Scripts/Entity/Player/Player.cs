using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Player : StateMachineEntity<Player, Player.State>, ISubject<Player.ObserverType, PlayerData>, IHealth, IBuff<PlayerData>
{
    //private StateMachine<Player, Telegram<State>> m_stateMachine;

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


    BuffComponent m_buffComponent;
    public BuffComponent BuffComponent { get { return m_buffComponent; } }


    ActionJoystick actionJoycstick;
    public ActionJoystick ActionJoystick { get { return actionJoycstick; } }

    MoveJoystick moveJoystick;
    public MoveJoystick MoveJoystick { get { return moveJoystick; } }

    // 조이스틱 프로퍼티
    public Vector2 ActionVec { get { return actionJoycstick.MainVec; } }

    public Vector2 MoveVec { get { return moveJoystick.ReturnMoveVec(); } }


    public Action<Collision2D> ContactAction;

    [SerializeField]
    PlayerData _data;
    public PlayerData Data { get { return _data; } }

    public void ResetPlayerData(PlayerData data) => _data = data;

    public enum State
    {      
        Dash,
        AttackReady,
        Attack,
        Move,
        Die,
        Reflect,
        Battle
    }

    public enum ObserverType
    {
       ShowDashUI,

       ShowRushUI,
       HideRushUI,

       Damaged,
       Die,
       Heal,
       Attack,
       AttackCancle,
       AttackReady
    }

    float minJoyVal = 0.25f;

    //스테이트들을 보관
    //private Dictionary<State, IState<Player, Telegram<State>>> m_dicState = new Dictionary<State, IState<Player, Telegram<State>>>();
    private List<IObserver<ObserverType, PlayerData>> m_observers = new List<IObserver<ObserverType, PlayerData>>();

    private void Awake()
    {
        m_dashComponent = GetComponent<DashComponent>();
        m_attackComponent = GetComponent<AttackComponent>();
        m_moveComponent = GetComponent<MoveComponent>();
        m_reflectComponent = GetComponent<ReflectComponent>();
        m_battleComponent = GetComponent<BattleComponent>();
        m_animator = GetComponent<Animator>();


        m_battleComponent.LootingSkill
            (DatabaseManager.Instance.UtilizationDB.SkillCallDatas.Find(x => x.Name == "Punch").CopyData());


        m_contactComponent = GetComponent<ContactComponent>();
        m_buffComponent = GetComponent<BuffComponent>();

        moveJoystick = GameObject.FindWithTag("MoveJoystick").GetComponent<MoveJoystick>();
        actionJoycstick = GameObject.FindWithTag("AttackJoystick").GetComponent<ActionJoystick>();
        
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

    private void Start()
    {
        //상태 생성
        IState<Player, State> move = new StatePlayerMove(this);
        IState<Player, State> attack = new StatePlayerAttack(this);
        IState<Player, State> attackReady = new StatePlayerAttackReady(this);
        IState<Player, State> dash = new StatePlayerDash();
        IState<Player, State> dead = new StatePlayerDie();
        IState<Player, State> reflect = new StatePlayerReflect();

        IState<Player, State> global = new StatePlayerGlobal();

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        m_dicState.Add(State.Move, move);
        m_dicState.Add(State.Attack, attack);
        m_dicState.Add(State.AttackReady, attackReady);
        m_dicState.Add(State.Dash, dash);
        m_dicState.Add(State.Die, dead);
        m_dicState.Add(State.Reflect, reflect);

        SetUp(this, State.Move);
        SetGlobalState(global);
    }

    public bool CanUseDash()
    {
        if (Data.DashRatio - 1 / Data.MaxDashCount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckOverMinValue(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > minJoyVal || Mathf.Abs(dir.y) > minJoyVal) return true;// 공격인 경우
        else return false;
    }

    // Update is called once per frame
    private void Update()
    {
        DoOperateUpdate();
        // 움직임이 감지되면 move state로 넘어감
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("DropItem"))
        {
            m_buffComponent.AddBuff("SpeedDebuff");

            DropSkill dropSkill = col.GetComponent<DropSkill>();
            m_battleComponent.LootingSkill(dropSkill.ReturnSkill());
        }
    }

    private void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        ContactComponent.CallWhenCollisionEnter(col);

        if (ContactAction != null) ContactAction(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        ContactComponent.CallWhenCollisionExit(col);
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




    public bool IsTarget(EntityTag tag)
    {
        return inheritedTag == tag;
    }

    public void UnderAttack(float healthPoint)
    {
        throw new System.NotImplementedException();
    }

    public void Heal(float healthPoint)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }

    public EntityTag ReturnTag()
    {
        return inheritedTag;
    }

    public void Knockback(Vector2 dir, float thrust)
    {
        throw new NotImplementedException();
    }

    public PlayerData GetData()
    {
        return Data;
    }
}
