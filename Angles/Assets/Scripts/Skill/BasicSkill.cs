using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : UnitaskUtility
{
    [SerializeField]
    SkillName skillName;
    public SkillName SkillName { get { return skillName; } set { skillName = value; } }

    [SerializeField]
    SkillUseType skillUseType;
    public SkillUseType SkillUseType { get { return skillUseType; } set { skillUseType = value; } }

    protected BattleComponent loadBattle;

    protected BasicEffect effect;

    protected float disableTime = 3f;

    protected int layerMask;

    protected virtual void Awake()
    {
        layerMask = LayerMask.GetMask("Enemy", "Player");
        bool get = TryGetComponent(out BasicEffect basicEffect);
        if (get == true) effect = basicEffect;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected void DisableObject() => gameObject.SetActive(false);

    protected override void OnDisable()
    {
        base.OnDisable();
        ObjectPooler.ReturnToPool(gameObject);
    }

    protected bool CheckCanHitSkill(string tag)
    {
        if (DatabaseManager.Instance.damageDictionary[SkillName].CheckTags(tag) == true) return true;
        else return false;
    }

    protected float ReturnDamage()
    {
        return DatabaseManager.Instance.damageDictionary[SkillName].damage;
    }

    protected void DamageToEntity(GameObject go)
    {
        Entity entity = go.GetComponent<Entity>();
        Vector2 dirToEnemy = go.transform.position - transform.position;

        entity.GetHit(ReturnDamage(), dirToEnemy);
    }

    protected GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    }

    public virtual void PlaySkill(SkillSupportData suportDatas) // 플레이어 스킬
    {
        
    }

    public virtual void PlayBasicSkill(Transform tr) // 적 스킬
    {

    }
}
