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
    protected SkillData data; // --> ������, ���� ��� ����� ������ �־�����
    public SkillData Data{ get { return data; } }

    //--> ������ ����Ʈ �����͵� �ʿ��� ��

    // �߰��� �� �� �ִ� �� --> ��� ����, 

    [SerializeField]
    DamageMethod damageMethod; // ���Ÿ� ����, �ٰŸ� ���� ���
    public DamageMethod DamageMethod { get { return damageMethod; } }

    [SerializeField]
    IEffectMethod effectMethod; // ȿ���� ����
    public IEffectMethod EffectMethod { get { return effectMethod; } }

    public void Execute(BattleComponent battle)
    {
        damageMethod.Attack(battle.gameObject, data);
    }

    //public void Execute(BattleComponent battle, SkillUseType useType)
    //{
    //    if (data.UseType != useType) return; // -->  ���� üũ

    //    //damageMethod.Attack(battle.gameObject, data);
    //    //effectMethod.Show(battle.gameObject);

    //    // ������Ʈ Ǯ������ ��ų ������ ���
    //    //Init(this);
    //}

    //public void Execute(BattleComponent battle, string name)
    //{
    //    if (data.Name != name) return; // -->  ���� üũ

    //    //damageMethod.Attack(battle.gameObject, data);
    //    //effectMethod.Show(battle.gameObject);

    //    // ������Ʈ Ǯ������ ��ų ������ ���
    //    //Init(this);
    //}

    //protected GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    //{
    //    return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    //}
}

///// <summary>
///// ����ص� ��ų ��� ī��Ʈ�� �� ����
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
//        // ��� ����
//    }
//}

///// <summary>
///// ����ϸ� ��ų ��� ī��Ʈ�� �پ����
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
//        // ��� ����
//    }
//}

///// <summary>
///// ȹ���ϸ� ��ų ��� ī��Ʈ�� �ö�
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