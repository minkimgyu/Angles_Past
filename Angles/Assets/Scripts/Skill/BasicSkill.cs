using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : UnitaskUtility
{
    [SerializeField]
    SkillData skillData;

    public SkillData SkillData
    {
        get
        {
            return skillData;
        }
        set
        {
            skillData = value;
        }
    }

    protected Transform moveTr;
    protected BattleComponent loadBattle;

    protected override void OnEnable()
    {
        base.OnEnable();
        Invoke("DisableObject", 5f);
    }

    protected virtual void DisableObject() => gameObject.SetActive(false);

    protected override void OnDisable()
    {
        moveTr = null;
        ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면
        base.OnDisable();
    }

    protected GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    }

    public virtual void Init(Transform tr, BattleComponent battleComponent)
    {
        moveTr = tr;
        loadBattle = battleComponent;
        transform.position = tr.position;
        transform.rotation = tr.rotation;
    }

    public virtual void PlaySkill(Vector2 dir, List<Collision2D> entity)
    {

        skillData.SkillUseCount -= 1;
        if(skillData.SkillUseCount < 1)
        {
            loadBattle.RemoveSkillFromLoad(this);
        }
    }
}
