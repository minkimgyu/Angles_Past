using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class UnitaskUtility
{
    protected bool nowRunning;
    public bool NowRunning
    {
        get { return nowRunning; }
        set { nowRunning = value; }
    }

    protected float waitTIme = 0.01f;

    public float WaitTIme
    {
        get { return WaitTIme; }
        set { waitTIme = value; }
    }

    public CancellationTokenSource source = new CancellationTokenSource();
    public Action cancelFn;

    public void CancelTask()
    {
        if(nowRunning == true)
        {
            source.Cancel();
            source = new CancellationTokenSource();
            cancelFn();
            nowRunning = false;
        }
    }

    public void WhenDestroy()
    {
        source.Cancel();
        source.Dispose();
    }

    public void WhenDisable()
    {
        source.Cancel();
    }

    public void WhenEnable()
    {
        if (source != null) source.Dispose();
        source = new CancellationTokenSource();
    }
}
