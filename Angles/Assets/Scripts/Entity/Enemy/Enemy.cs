using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Enemy<T> : Avatar<T>
{
    protected BuffInt score;
    public BuffInt Score { get { return score; }}

    protected virtual void Start() => AddState();

    protected abstract void AddState();

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
    BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames, BuffInt score)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames);

        this.score = score.CopyData();
    }
}