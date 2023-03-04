using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : Entity
{
    Player player;
    public FollowComponent followComponent;
    public BasicReflectComponent basicReflectComponent;
    public ForceComponent forceComponent;

    public EnemyData enemyData;

    public Action dieAction;

    string dieEffect;

    public EnemyData EnemyData
    {
        get
        {
            return enemyData;
        }
        set
        {
            enemyData = value;
            if (rigid == null) return;

            rigid.mass = enemyData.Weight;
            rigid.drag = enemyData.Drag;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        Transform tempTr = transform.GetChild(0);
        if (tempTr.CompareTag("InnerColor") == false) return;

        innerImage = tempTr.GetComponent<SpriteRenderer>();
    }
    public void Init(EnemyData enemyData)
    {
        EnemyData = enemyData;
        hp = enemyData.Hp;
        dieEffect = ReturnDieEffectName(EnemyData.Shape);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        player = PlayManager.Instance.player;
        followComponent = GetComponent<FollowComponent>();
        forceComponent = GetComponent<ForceComponent>();
        basicReflectComponent = GetComponent<BasicReflectComponent>();
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }

    public override void GetHit(float damage, Vector3 dir)
    {
        base.GetHit(damage);
        followComponent.WaitFollow();
        basicReflectComponent.KnockBack(dir);
    }

    string ReturnDieEffectName(string shape)
    {
        List<AdditionalPrefabData> prefabDatas = DatabaseManager.Instance.EntityDB.AdditionalPrefab;

        for (int i = 0; i < prefabDatas.Count; i++)
        {
            if (prefabDatas[i].Name.Contains(shape))
            {
                return prefabDatas[i].Name;
            }
        }

        return null;
    }

    protected override void Die()
    {
        if(dieAction != null) dieAction();

        GameObject go = ObjectPooler.SpawnFromPool(dieEffect, transform.position, transform.rotation);
        go.GetComponent<DieEffect>().Init(enemyData.Color);
        base.Die();
    }
}