using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

abstract public class BasicSkill : MonoBehaviour
{
    [SerializeField]
    SkillData skillData;
    public SkillData SkillData { get { return skillData; } set { skillData = value; } }

    string skillName;

    DamageComponent damageComponent;
    BasicEffect effect;

    CancellationTokenSource _source = new();

    protected int layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemy", "Player");

        skillData = DatabaseManager.Instance.ReturnSkillData(skillName);
        damageComponent = GetComponent<DamageComponent>();
        effect = GetComponent<BasicEffect>();
    }

    protected void DisableObject() => gameObject.SetActive(false);

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }

    //protected void Attack(List<GameObject> gos, float knockBackThrust)
    //{
    //    Entity entity = go.GetComponent<Entity>();
    //    Vector2 dirToEnemy = (go.transform.position - transform.position).normalized;

    //    //go 발사 위치, tr 맞는 위치
    //    int layerMask = skillData.ReturnLayerMask();
    //    print(layerMask);
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToEnemy, 20, layerMask); // 맞은애 레이어 가져오기
    //    Debug.DrawRay(transform.position, dirToEnemy, Color.red, 1f);

    //    if (hit.collider != null)
    //    {
    //        GetEffectUsingName("HitEffect", hit.point, Quaternion.identity);
    //    }

    //    entity.GetHit(skillData.Damage, dirToEnemy * knockBackThrust);
    //}

    public virtual void Attack(List<GameObject> gos, float damage, float knockBackThrust, List<EntityTag> entityTags)
    {
        damageComponent.DamageToEntity(gos, damage, knockBackThrust, entityTags);
    }

    public virtual void Attack(List<GameObject> gos, float damage, float knockBackThrust)
    {
        damageComponent.DamageToEntity(gos, damage, knockBackThrust);
    }

    public abstract void PlayEffect();

    GameObject GetEffectUsingName(string name, Vector3 pos, Quaternion rotation, Transform tr = null)
    {
        return ObjectPooler.SpawnFromPool(name, pos, rotation, tr);
    }

    //public virtual void PlayAddition()
    //{
    //    print("PlayAdditionalSkill");
    //}

    //public virtual void PlayCountUp(SkillData data)
    //{
    //    print("PlayCountUp");
    //}
}
