using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class Player : Entity
{
    PlayerMode playerMode;

    public PlayerMode PlayerMode
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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
}
