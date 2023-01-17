using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public float attackThrust = 5;

    WaitForSeconds wait = new WaitForSeconds(3f);

    public bool nowAttack = false;
    Coroutine attackCor = null;

    public VariableJoystick moveJoy;

    Rigidbody2D rigidbody2;
    Vector3 moveVec;

    Vector3 loadMove1;
    Vector3 loadMove2;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                if(attackCor != null)
                {
                    StopCoroutine(attackCor);
                    nowAttack = false;
                }
            }
        }
        else
        {
            MoveAction();
        }
    }

    public void Attack(Vector3 attack)
    {
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

        rigidbody2.AddForce(attack * attackThrust, ForceMode2D.Impulse);
        yield return wait;
        nowAttack = false;

        attackCor = null;
    }

    public void MoveActionWhenAttack()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float x = moveJoy.Horizontal;
        float y = moveJoy.Vertical;

        if (x != 0 && y != 0)
        {
            Move(h, v);

            if (attackCor != null)
            {
                StopCoroutine(attackCor);
                nowAttack = false;
            }
        }
        else if((h != 0 && v != 0))
        {
            Move(x, y);

            if (attackCor != null)
            {
                StopCoroutine(attackCor);
                nowAttack = false;
            }
        }
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
        moveVec.Set(x, y, 0);
        rigidbody2.velocity = moveVec * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
