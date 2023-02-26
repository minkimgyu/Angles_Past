using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicReflectComponent : MonoBehaviour
{
    protected Entity entity;
    protected ForceComponent forceComponent;
    FollowComponent followComponent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        followComponent = GetComponent<FollowComponent>();
        forceComponent = GetComponent<ForceComponent>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        ReflectEntity(col);
        ReflectWithEnemy(col.gameObject);
    }

    void ReflectWithEnemy(GameObject go)
    {
        if (go.CompareTag("Enemy") && gameObject.CompareTag("Enemy")) // 본인이 적이고 상대방도 적일 경우
        {
            FollowComponent goFollowComponent = go.GetComponent<FollowComponent>();
            Entity entity = go.GetComponent<Entity>();

            Vector2 goVec = entity.rigid.velocity * DatabaseManager.Instance.ThrustRatio;

            if (followComponent.NowHit == false && goFollowComponent.NowHit == true) // 나는 아직 맞지 않았고 상대는 맞은 경우
            {
                followComponent.WaitFollow();
                go.GetComponent<BasicReflectComponent>().KnockBack(goVec);
            }
        }
    }

    public virtual void ReflectEntity(Collision2D col)
    {

    }

    public void KnockBack(Vector2 dir1)
    {
        //float ratio = (dir1 / dir1.normalized).magnitude * 1.2f;

        forceComponent.CancelTask();
        forceComponent.AddForceUsingVec(dir1);
    }
}
