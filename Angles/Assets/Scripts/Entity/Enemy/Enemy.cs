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

    public EnemyData enemyData;

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
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        player = PlayManager.Instance.player;
        followComponent = GetComponent<FollowComponent>();
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

    protected override void Die()
    {
        base.Die();
        ObjectPooler.SpawnFromPool("TriangleDieEffect", transform.position, transform.rotation);
    }
}