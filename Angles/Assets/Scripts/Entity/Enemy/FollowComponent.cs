using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowComponent : UnitaskUtility
{
    //Entity entity;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    entity = GetComponent<Entity>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //public void MoveToPlayer()
    //{
    //    if (nowStop == true) return;

    //    float distance = Vector2.Distance(playerRigid.position, entity.rigid.position);
    //    if (distance < maxFollowDistance)
    //    {
    //        entity.rigid.velocity = Vector2.zero;
    //        return;
    //    }

    //    Vector2 dirVec = playerRigid.position - entity.rigid.position;
    //    Vector2 nextVec = dirVec.normalized * DatabaseManager.Instance.FollowSpeed * Time.fixedDeltaTime;
    //    entity.rigid.MovePosition(entity.rigid.position + nextVec);
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    ObjectPooler.ReturnToPool(gameObject);
    //}
}
