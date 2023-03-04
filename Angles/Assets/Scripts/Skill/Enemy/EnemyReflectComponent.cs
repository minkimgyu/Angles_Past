using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReflectComponent : BasicReflectComponent
{
    FollowComponent followComponent;
    Enemy myEnemy;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        myEnemy = GetComponent<Enemy>();
        followComponent = GetComponent<FollowComponent>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        ReflectWithEnemy(col.gameObject);
    }

    void ReflectWithEnemy(GameObject go)
    {
        if (go.CompareTag(EntityTag.Enemy.ToString()) == true) // 상대방이 적일 경우
        {
            Enemy goEnemy = go.GetComponent<Enemy>();
            FollowComponent goFollowComponent = goEnemy.followComponent;

            Vector2 dir = transform.position - goEnemy.transform.position;
            Vector2 vec = dir * goEnemy.rigid.velocity.magnitude * myEnemy.enemyData.KnockBackThrust;

            if (goFollowComponent.NowHit == true && myEnemy.followComponent.NowHit == false) // 상대는 맞은 상태 나는 안 맞은 상태
            {
                print(myEnemy.enemyData.KnockBackDamage);
                myEnemy.GetHit(myEnemy.enemyData.KnockBackDamage, vec);
            }
        }
    }
}
