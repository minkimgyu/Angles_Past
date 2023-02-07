using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rigid;
    public Action fixedUpdateAction;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        fixedUpdateAction();
    }
}
