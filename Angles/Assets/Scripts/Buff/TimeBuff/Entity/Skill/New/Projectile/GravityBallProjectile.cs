using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBallProjectile : BasicSpawnedObject, IProjectile
{
    public List<Rigidbody2D> rigidbodies;

    EntityTag[] disableTargetTag;

    [SerializeField]
    float absorbThrust;

    bool nowShoot = false;

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

    public void Inintialize(float disableTime, string[] skillNames, EntityTag[] hitTargetTag, EntityTag[] disableTargetTag, float absorbThrust)
    {
        Inintialize(disableTime, skillNames);
        this.hitTargetTag = hitTargetTag;
        this.disableTargetTag = disableTargetTag;
        this.absorbThrust = absorbThrust;
        nowShoot = false;
    }

    public void Shoot(Vector2 dir, float thrust)
    {
        nowShoot = true;
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
        if(IsTarget(col.gameObject, hitTargetTag)) rigidbodies.Add(col.GetComponent<Rigidbody2D>()); // hitTag에 들어가있는 

        if(IsTarget(col.gameObject, disableTargetTag) && nowShoot == true) DisableObject();
    }

    public override void DisableObject()
    {
        m_skillController.UseSkill(BaseSkill.UseConditionType.End);
        base.DisableObject();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (IsTarget(col.gameObject, hitTargetTag)) rigidbodies.Remove(col.GetComponent<Rigidbody2D>());
    }
}
