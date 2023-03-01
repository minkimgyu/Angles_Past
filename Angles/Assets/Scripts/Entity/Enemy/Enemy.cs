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

    // Start is called before the first frame update
    protected override void Start()
    {
        player = PlayManager.Instance.player;
        followComponent = GetComponent<FollowComponent>();
        basicReflectComponent = GetComponent<BasicReflectComponent>();
    }

    public void Init(EnemyData data)
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyData = data;
        rigid.mass = enemyData.Weight;
        rigid.drag = enemyData.Drag;
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
}