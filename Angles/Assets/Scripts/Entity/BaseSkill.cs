using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectMethod
{
    public void Show(GameObject go);
}

[CreateAssetMenu(fileName = "BaseSkill", menuName = "Scriptable Object/BaseSkill", order = int.MaxValue)]
[System.Serializable]
public class BaseSkill : ScriptableObject
{
    [SerializeField]
    protected SkillData data; // --> 데미지, 범위 등등 공통된 변수만 넣어주자
    public SkillData Data{ get { return data; } }

    //--> 앞으로 이펙트 데이터도 필요할 것

    // 추가로 들어갈 수 있는 것 --> 사용 조건, 

    [SerializeField]
    DamageMethod damageMethod; // 원거리 공격, 근거리 공격 등등
    public DamageMethod DamageMethod { get { return damageMethod; } }

    [SerializeField]
    IEffectMethod effectMethod; // 효과들 모음
    public IEffectMethod EffectMethod { get { return effectMethod; } }

    public void Execute(BattleComponent battle)
    {
        damageMethod.Attack(battle.gameObject, data);
    }

    //public void Execute(BattleComponent battle, SkillUseType useType)
    //{
    //    if (data.UseType != useType) return; // -->  조건 체크

    //    //damageMethod.Attack(battle.gameObject, data);
    //    //effectMethod.Show(battle.gameObject);

    //    // 오브젝트 풀링에서 스킬 꺼내서 사용
    //    //Init(this);
    //}

    //public void Execute(BattleComponent battle, string name)
    //{
    //    if (data.Name != name) return; // -->  조건 체크

    //    //damageMethod.Attack(battle.gameObject, data);
    //    //effectMethod.Show(battle.gameObject);

    //    // 오브젝트 풀링에서 스킬 꺼내서 사용
    //    //Init(this);
    //}

    //protected GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    //{
    //    return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    //}
}

///// <summary>
///// 사용해도 스킬 사용 카운트가 안 줄음
///// </summary>
///// 
////[CreateAssetMenu(fileName = "InnumerableSkill", menuName = "Scriptable Object/Skill/InnumerableSkill", order = int.MaxValue)]
//[System.Serializable]
//public class InnumerableSkill : BaseSkill
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
////[CreateAssetMenu(fileName = "CountableSkill", menuName = "Scriptable Object/Skill/CountableSkill", order = int.MaxValue)]
//[System.Serializable]
//public class CountableSkill : BaseSkill
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
//        if (IsCountZero()) battleComponent.RemoveSkillData(this);
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
//[System.Serializable]
//public class CountUpSkill : CountableSkill
//{
//    [SerializeField]
//    int upCount;

//    void CountUp()
//    {
//        count += upCount;
//    }

//    public override void Loot(BattleComponent battle)
//    {
//        CountUp();
//    }
//}