using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : UnitaskUtility
{
    [SerializeField]
    SkillData skillData;
    public SkillData SkillData { get { return skillData; } set { skillData = value; } }

    protected BasicEffect effect;
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
        Init();
    }

    public virtual void Init()
    {
        SkillName skillName = (SkillName)System.Enum.Parse(typeof(SkillName), name);
        print(skillName);
        skillData = DatabaseManager.Instance.ReturnSkillData(skillName);
    }

    public virtual void PlayBasicSkill(Transform tr) // 적 스킬
    {

    }
}
