using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : UnitaskUtility
{
    [SerializeField]
    SkillName skillName;
    public SkillName SkillName { get { return skillName; } set { skillName = value; } }

    [SerializeField]
    SkillData skillData;
    public SkillData SkillData { get { return skillData; } set { skillData = value; } }

    protected BasicEffect effect;
    protected int layerMask;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public virtual void Init()
    {
        layerMask = LayerMask.GetMask("Enemy", "Player");
        bool get = TryGetComponent(out BasicEffect basicEffect);
        if (get == true) effect = basicEffect;

        skillData = DatabaseManager.Instance.ReturnSkillData(skillName);
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
    
    protected void DamageToEntity(GameObject go, float knockBackThrust)
    {
        Entity entity = go.GetComponent<Entity>();
        Vector2 dirToEnemy = (go.transform.position - transform.position).normalized;

        entity.GetHit(skillData.Damage, dirToEnemy * knockBackThrust);
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
