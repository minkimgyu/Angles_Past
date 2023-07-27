using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBallProjectile : BasicProjectile
{
    public List<Rigidbody2D> rigidbodies;

    [SerializeField]
    float endTime = 10;

    [SerializeField]
    float absorbThrust = 50;

    DashComponent dashComponent;

    protected override void Awake()
    {
        base.Awake();
        dashComponent = GetComponent<DashComponent>();
    }

    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        Invoke("NowFinish", endTime);
    }

    public void ShootBall(Vector2 dir, float thrust)
    { 
        dashComponent.PlayDash(dir, thrust);
    }

    public override void DoUpdate()
    {
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            Vector3 dir = -(rigidbodies[i].transform.position - transform.position).normalized * absorbThrust;
            rigidbodies[i].AddForce(dir, ForceMode2D.Force);
        }
    }

    private void Update()
    {
        DoUpdate();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            rigidbodies.Add(col.GetComponent<Rigidbody2D>());
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            rigidbodies.Remove(col.GetComponent<Rigidbody2D>());
        }
    }

    protected override void OnDisable()
    {
        CancelInvoke();
        base.OnDisable();
    }
}
