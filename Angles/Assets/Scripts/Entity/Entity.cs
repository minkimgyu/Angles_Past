using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    float hp;

    public float Hp
    {
        get { return hp; }
        set 
        {
            hp = value;
            if (hp < 0)
            {
                hp = 0;
                Die();
            }
        }
    }

    ActionMode playerMode = ActionMode.Idle;

    public ActionMode PlayerMode
    {
        get
        {
            return playerMode;
        }
        set
        {
            playerMode = value;
        }
    }

    Animator animator;

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

    public float moveSpeed;
    public Rigidbody2D rigid;
    public Action fixedUpdateAction;

    public Action<Collision2D> collisionEnterAction;
    public Action<Collision2D> collisionExitAction;

    [SerializeField]
    private EntityData entityData;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        fixedUpdateAction();
    }

    public virtual void Init(EntityData data)
    {
        entityData = data;
    }

    protected virtual void Die()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (collisionEnterAction != null) collisionEnterAction(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (collisionExitAction != null) collisionExitAction(col);
    }
}
