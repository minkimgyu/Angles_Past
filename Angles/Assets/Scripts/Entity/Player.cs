using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class Player : Entity
{
    public float attackThrust = 5;

    public float dashThrust = 8;

    WaitForSeconds attackWait = new WaitForSeconds(0.4f);
    WaitForSeconds dashWait = new WaitForSeconds(0.1f);

    public bool nowAttack = false;
    Coroutine attackCor = null;

    public bool nowDash = false;
    Coroutine dashCor = null;

    Coroutine resetCor = null;

    public VariableJoystick moveJoy;
    public AttackJoystick attackJoy;

    public Vector3 moveVec;

    Vector3 loadMove1;
    Vector3 loadMove2;

    Animator animator;

    float angle = 0;

    public PlayerDash playerDash;

    public Animator Animator
    {
        get
        {
            return animator;
        }
        set
        {
            if (value != null) animator = value;
        }
    }

    public Action UpdateAction;

    public InputComponent inputComponent;
    public Dash dash;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RotateUsingVelocity();
        UpdateAction();
    }

    void RotateUsingVelocity()
    {
        float tempAngle = angle;

        angle = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg;

        if (angle == tempAngle) return;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        if (nowAttack == true)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            float x = moveJoy.Horizontal;
            float y = moveJoy.Vertical;

            float offset = 0.35f;

            float KeyX = Mathf.Abs(loadMove1.x - h);
            float KeyY = Mathf.Abs(loadMove1.y - v);

            float JoyX = Mathf.Abs(loadMove2.x - x);
            float JoyY = Mathf.Abs(loadMove2.y - y);



            if (KeyX >= offset || KeyY >= offset || JoyX >= offset || JoyY >= offset)
            {
                if (attackCor != null)
                {
                    StopCoroutine(attackCor);
                    attackCor = null;
                    nowAttack = false;
                    animator.SetBool("NowAttack", false);
                }
            }
        }
        else if(nowAttack == false && nowDash == false)
        {
            MoveAction();
        }
    }

    public void Attack(Vector3 attack)
    {
        animator.SetBool("NowReady", false);
        animator.SetBool("NowAttack", true);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float x = moveJoy.Horizontal;
        float y = moveJoy.Vertical;

        loadMove1.Set(h, v, 0);
        loadMove2.Set(x, y, 0);

        if(attackCor != null)
        {
            StopCoroutine(attackCor);
            attackCor = null;
            nowAttack = false;
        }

        attackCor = StartCoroutine(CallAttack(attack));
    }

    IEnumerator CallAttack(Vector3 attack)
    {
        nowAttack = true;

        attack.Set(-attack.x, -attack.y, 0);

        rigid.AddForce(attack * attackThrust, ForceMode2D.Impulse);
        yield return null;

        ResetAttack();
    }

    public void Dash()
    {
        if(nowAttack == true)
        {
            StopAttack();

            Vector2 tempVec = rigid.velocity.normalized;
            rigid.velocity = Vector2.zero;
            dashCor = StartCoroutine(CallDash(tempVec, dashThrust));
            return;
        }


        animator.SetTrigger("NowDash");

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float x = moveJoy.Horizontal;
        float y = moveJoy.Vertical;

        float offset = 0;

        float KeyX = Mathf.Abs(h);
        float KeyY = Mathf.Abs(v);

        float JoyX = Mathf.Abs(x);
        float JoyY = Mathf.Abs(y);


        if (KeyX >= offset || KeyY >= offset)
        {
            Vector3 dash = new Vector3(h, v, 0);

            if (dashCor != null)
            {
                StopCoroutine(dashCor);
                dashCor = null;
                nowDash = false;
            }

            dashCor = StartCoroutine(CallDash(dash, dashThrust));

        }
        else if (JoyX >= offset || JoyY >= offset)
        {
            Vector3 dash = new Vector3(x, y, 0);

            if (dashCor != null)
            {
                StopCoroutine(dashCor);
                dashCor = null;
                nowDash = false;
            }

            dashCor = StartCoroutine(CallDash(dash, dashThrust));
        }
       
    }

    IEnumerator CallDash(Vector3 dash, float dashThrust)
    {
        nowDash = true;
        dash.Set(dash.x, dash.y, 0);
        rigid.AddForce(dash * dashThrust, ForceMode2D.Impulse);

        yield return dashWait;

        nowDash = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (nowAttack == false) return;

        if (collision.gameObject.CompareTag("Wall") == true)
        {
            WallColorChange colorChange =  collision.gameObject.GetComponent<WallColorChange>();

            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

                colorChange.ChangeTileColor(hitPosition);

                //tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }



            Vector2 velocity = rigid.velocity;
            var speed = velocity.magnitude;

            speed = 7;
            var dir = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal);
            rigid.velocity = dir * speed;


            ResetAttack();
        }
    }



    public void StopAttack()
    {
        if (resetCor != null)
        {
            StopCoroutine(resetCor);
            resetCor = null;
        }
    }

    public void ResetAttack()
    {
        StopAttack();
        resetCor = StartCoroutine(ResetAttackCo());
    }

    IEnumerator ResetAttackCo()
    {
        yield return attackWait;

        nowAttack = false;
        attackCor = null;
        animator.SetBool("NowAttack", false);
    }

    public void MoveAction()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float x = moveJoy.Horizontal;
        float y = moveJoy.Vertical;

        if (x == 0 && y == 0)
        {
            Move(h, v);

            if (attackCor != null)
            {
                StopCoroutine(attackCor);
                nowAttack = false;
            }
        }
        else
        {
            Move(x, y);

            if (attackCor != null)
            {
                StopCoroutine(attackCor);
                nowAttack = false;
            }
        }
    }

    public void Move(float x, float y)
    {
        if(attackJoy.NowAttackReady == false)
        {
            moveVec.Set(x, y, 0);
            rigid.velocity = moveVec * moveSpeed;
        }
        else
        {
            moveVec.Set(x, y, 0);
            rigid.velocity = moveVec * (moveSpeed / 2);
        }
    }
}
