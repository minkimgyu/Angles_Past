using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBallProjectile : BasicSpawnedObject, IProjectile
{
    public List<Rigidbody2D> rigidbodies;

    [SerializeField]
    float absorbThrust;

    public DashComponent DashComponent { get; set; }

    protected override void Awake()
    {
        base.Awake();
        DashComponent = GetComponent<DashComponent>();
    }

    public override void ResetObject(Vector3 pos)
    {
        base.ResetObject(pos);
        transform.position = pos;
    }

    public void Inintialize(float disableTime, string[] skillNames, string[] hitTargetTag, float absorbThrust)
    {
        Inintialize(disableTime, skillNames);
        this.hitTargetTag = hitTargetTag;
        this.absorbThrust = absorbThrust;
    }

    public void Shoot(Vector2 dir, float thrust)
    {
        DashComponent.PlayDash(dir, thrust); // 슈팅 소환이면 날려보내기 아니면 그냥 소환만 진행
    }

    protected override void DoUpdate()
    {
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            Vector3 dir = -(rigidbodies[i].transform.position - transform.position).normalized * absorbThrust;
            rigidbodies[i].AddForce(dir, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(IsTarget(col.gameObject)) rigidbodies.Add(col.GetComponent<Rigidbody2D>());
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (IsTarget(col.gameObject)) rigidbodies.Remove(col.GetComponent<Rigidbody2D>());
    }
}
