using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    protected float hp;

    public float Hp
    {
        get { return hp; }
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
    public SpriteRenderer innerImage;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (fixedUpdateAction != null) fixedUpdateAction();
    }

    public virtual void GetHit(float damage)
    {
        hp -= damage;
        if (hp <= 0) Die();
    }

    public virtual void GetHit(float damage, Vector3 dir)
    {
        hp -= damage;
    }

    public virtual void GetHeal(float healPoint)
    {
        hp += healPoint;
    }

    public void StopMove()
    {
        rigid.velocity = Vector2.zero;
    }

    protected void DisableObject() => gameObject.SetActive(false);

    protected virtual void Die()
    {
        DisableObject();
    }
}
