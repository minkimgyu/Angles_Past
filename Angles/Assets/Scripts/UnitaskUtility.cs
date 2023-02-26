using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class UnitaskUtility : MonoBehaviour
{
    protected bool nowRunning;
    public bool NowRunning
    {
        get { return nowRunning; }
    }

    float waitTIme = 0.01f;

    protected  float WaitTIme
    {
        get { return waitTIme; }
        set 
        {
            waitTIme = value;
        }
    }

    public CancellationTokenSource source = new CancellationTokenSource();
    protected Action cancelFn;

    public void CancelTask()
    {
        if(nowRunning == true)
        {
            source.Cancel();
            source = new CancellationTokenSource();
            if(cancelFn != null) cancelFn();
            nowRunning = false;
        }
    }

    private void OnDestroy()
    {
        WhenDestroy();
    }

    protected virtual void OnDisable()
    {
        WhenDisable();
    }

    protected virtual void OnEnable()
    {
        WhenEnable();
    }

    void WhenDestroy()
    {
        source.Cancel();
        source.Dispose();
    }

    void WhenDisable()
    {
        source.Cancel();
    }

    protected void WhenEnable()
    {
        if (source != null) source.Dispose();
        source = new CancellationTokenSource();
    }
}
