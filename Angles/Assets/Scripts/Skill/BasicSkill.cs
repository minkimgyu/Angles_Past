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

    protected BasicBattleComponent battleComponent;
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
        if(battleComponent != null) battleComponent.RemoveSkillInList(this);
        battleComponent = null;
        base.OnDisable();
        ObjectPooler.ReturnToPool(gameObject);
    }
    
    protected void DamageToEntity(GameObject go, float knockBackThrust)
    {
        Entity entity = go.GetComponent<Entity>();
        Vector2 dirToEnemy = (go.transform.position - transform.position).normalized;

        //go 발사 위치, tr 맞는 위치
        int layerMask = skillData.ReturnLayerMask();
        print(layerMask);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToEnemy, 20, layerMask); // 맞은애 레이어 가져오기
        Debug.DrawRay(transform.position, dirToEnemy, Color.red, 1f);

        if (hit.collider != null)
        {
            GetEffectUsingName("HitEffect", hit.point, Quaternion.identity);
        }

        entity.GetHit(skillData.Damage, dirToEnemy * knockBackThrust);
    }

    protected GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    }

    public virtual void PlayAddition()
    {
        print("PlayAdditionalSkill");
    }

    public virtual void PlayCountUp(SkillData data)
    {
        print("PlayCountUp");
    }

    public virtual void PlaySkill(SkillSupportData suportDatas, BasicBattleComponent battleComponent) // 플레이어 스킬
    {
        this.battleComponent = battleComponent;
    }

    public virtual void PlaySkill(Transform tr, BasicBattleComponent battleComponent) // 적 스킬
    {
        this.battleComponent = battleComponent;
    }
}
