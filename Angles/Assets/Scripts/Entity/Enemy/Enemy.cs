using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : Entity
{
    Player player;
    FollowComponent followComponent;
    BasicReflectComponent basicReflectComponent;

    // Start is called before the first frame update
    protected override void Start()
    {
        player = PlayManager.Instance.player;
        followComponent = GetComponent<FollowComponent>();
        basicReflectComponent = GetComponent<BasicReflectComponent>();
        base.Start();
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