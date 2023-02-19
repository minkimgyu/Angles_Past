using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class Player : Entity
{
    [SerializeField]
    SpawnAssistant spawnAssistant;
    public SpawnAssistant SpawnAssistant
    {
        get
        {
            return spawnAssistant;
        }
    }

    private void Awake()
    {
        spawnAssistant = GetComponentInChildren<SpawnAssistant>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
