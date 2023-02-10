using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : UnitaskUtility
{
    [SerializeField]
    SkillName skillName;
    public SkillName SkillName
    {
        get
        {
            return skillName;
        }
        set
        {
            skillName = value;
        }
    }

    [SerializeField]
    SkillUseType skillUseType;
    public SkillUseType SkillUseType
    {
        get
        {
            return skillUseType;
        }
        set
        {
            skillUseType = value;
        }
    }

    protected Transform moveTr;

    protected virtual void OnEnable()
    {
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

    protected BasicEffect GetEffectUsingName(Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        BasicEffect skill = ObjectPooler.SpawnFromPool(SkillName.ToString() + "Effect", pos, rotation, tr).GetComponent<BasicEffect>();
        return skill;
    }

    public virtual void Init(Transform tr, Vector2 dir, List<Collision2D> entity)
    {
        moveTr = tr;
        transform.position = tr.position;
        transform.rotation = tr.rotation;

        PlaySkill(tr, dir, entity);
    }

    //private void Update()
    //{
    //    if (moveTr == null) return;

    //    transform.position = moveTr.position;
    //    transform.rotation = moveTr.rotation;
    //}

    public virtual void PlaySkill(Transform tr, Vector2 dir, List<Collision2D> entity)
    {

    }
}
