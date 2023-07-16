using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

// 이펙트는 오브젝트 풀링에서 꺼내서 적용

abstract public class BasicSkill : MonoBehaviour // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    [SerializeField]
    protected BaseSkill m_baseSkill;

    public abstract void Execute(BattleComponent battle); // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
}

public class OnceSkill : BasicSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public override void Execute(BattleComponent battle) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        m_baseSkill.EffectMethod.Show(battle.gameObject); // --> 이펙트 전달

        m_baseSkill.Execute(battle);
        ObjectPooler.ReturnToPool(gameObject);
    }
}

public class TickSkill : BasicSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    CancellationTokenSource _source = new();

    public override void Execute(BattleComponent battle) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        m_baseSkill.EffectMethod.Show(battle.gameObject); // --> 이펙트 전달

        DamageTask(battle).Forget();
    }

    private async UniTaskVoid DamageTask(BattleComponent battle)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(m_baseSkill.Data.PreDelay), cancellationToken: _source.Token);

        int storedTick = 0;

        if (m_baseSkill.Data.AttackTick == 1)
        {
            m_baseSkill.Execute(battle);
        }
        else
        {
            float delay = m_baseSkill.Data.AttackTick / m_baseSkill.Data.Duration;

            while (m_baseSkill.Data.AttackTick > storedTick)
            {
                m_baseSkill.Execute(battle);
                storedTick++;
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _source.Token);
            }
        }

        ObjectPooler.ReturnToPool(gameObject);
    }

    public void CancelTask()
    {
        _source.Cancel();
        _source = null;
        _source = new();
    }

    private void OnDestroy()
    {
        _source.Cancel();
        _source.Dispose();
    }

    private void OnEnable()
    {
        if (_source != null)
            _source.Dispose();

        _source = new();
    }

    private void OnDisable()
    {
        _source.Cancel();
    }
}

///// <summary>
///// 사용해도 스킬 사용 카운트가 안 줄음
///// </summary>
///// 
//public class InnumerableSkill : BasicSkill
//{
//    public override void Init(BattleComponent battle)
//    {

//    }

//    public override void Loot(BattleComponent battle)
//    {
//        // 비어 놓자
//    }
//}

///// <summary>
///// 사용하면 스킬 사용 카운트가 줄어들음
///// </summary>
///// 
//public class CountableSkill : BasicSkill
//{
//    BattleComponent battleComponent;

//    [SerializeField]
//    protected int count;

//    bool IsCountZero()
//    {
//        if (count > 0)
//        {
//            count -= 1;
//            if (count == 0) return true;
//        }

//        return false;
//    }

//    public override void Init(BattleComponent battle)
//    {
//        battleComponent = battle;
//    }

//    public override void Execute(BattleComponent battle)
//    {
//        base.Execute(battle);
//        if (IsCountZero()) battleComponent.RemoveSkillData(skill);
//    }

//    public override void Loot(BattleComponent battle)
//    {
//        // 비어 놓자
//    }
//}

///// <summary>
///// 획득하면 스킬 사용 카운트가 올라감
///// </summary>
///// 
////[CreateAssetMenu(fileName = "CountUpSkill", menuName = "Scriptable Object/Skill/CountUpSkill", order = int.MaxValue)]
//public class CountUpSkill : CountableSkill
//{
//    [SerializeField]
//    protected int upCount;

//    void CountUp()
//    {
//        count += upCount;
//    }

//    public override void Loot(BattleComponent battle)
//    {
//        CountUp();
//    }
//}

//[SerializeField]
//SkillData skillData;
//public SkillData SkillData { get { return skillData; } set { skillData = value; } }

//string skillName;

//DamageComponent damageComponent;
//BasicEffect effect;

//CancellationTokenSource _source = new();

//protected int layerMask;

//private void Awake()
//{
//    layerMask = LayerMask.GetMask("Enemy", "Player");

//    skillData = DatabaseManager.Instance.ReturnSkillData(skillName);
//    damageComponent = GetComponent<DamageComponent>();
//    effect = GetComponent<BasicEffect>();
//}

//protected void DisableObject() => gameObject.SetActive(false);

//private void OnDisable()
//{
//    ObjectPooler.ReturnToPool(gameObject);
//}

////protected void Attack(List<GameObject> gos, float knockBackThrust)
////{
////    Entity entity = go.GetComponent<Entity>();
////    Vector2 dirToEnemy = (go.transform.position - transform.position).normalized;

////    //go 발사 위치, tr 맞는 위치
////    int layerMask = skillData.ReturnLayerMask();
////    print(layerMask);
////    RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToEnemy, 20, layerMask); // 맞은애 레이어 가져오기
////    Debug.DrawRay(transform.position, dirToEnemy, Color.red, 1f);

////    if (hit.collider != null)
////    {
////        GetEffectUsingName("HitEffect", hit.point, Quaternion.identity);
////    }

////    entity.GetHit(skillData.Damage, dirToEnemy * knockBackThrust);
////}

//public virtual void Attack(List<GameObject> gos, float damage, float knockBackThrust, List<EntityTag> entityTags)
//{
//    damageComponent.DamageToEntity(gos, damage, knockBackThrust, entityTags);
//}

//public virtual void Attack(List<GameObject> gos, float damage, float knockBackThrust)
//{
//    damageComponent.DamageToEntity(gos, damage, knockBackThrust);
//}

//public abstract void PlayEffect();

//GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
//{
//    return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
//}

////public virtual void PlayAddition()
////{
////    print("PlayAdditionalSkill");
////}

////public virtual void PlayCountUp(SkillData data)
////{
////    print("PlayCountUp");
////}
