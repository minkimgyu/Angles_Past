using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class BasicUnitask
{
    bool nowRunning;
    public bool NowRunning
    {
        get { return nowRunning; }
        set { nowRunning = value; }
    }

    float waitTIme = 0.01f;
    public float WaitTime
    {
        get { return waitTIme; }
        set { waitTIme = value; }
    }

    public CancellationTokenSource source = new CancellationTokenSource();
    public Action cancelFn;

    public void CancelTask()
    {
        if (nowRunning == true)
        {
            source.Cancel();
            source = new CancellationTokenSource();
            if (cancelFn != null) cancelFn();
            nowRunning = false;
        }
    }
}

public class UnitaskUtility : MonoBehaviour
{
    protected Dictionary<string, BasicUnitask> tasks = new Dictionary<string, BasicUnitask>();

    protected string basicTask = "Task";

    public BasicUnitask BasicTask
    { 
        get 
        {
            if (tasks != null && tasks.Count > 0)
                return tasks[basicTask];
            else
                return null;
        } 
    }

    protected virtual void Awake()
    {
        AddTask(basicTask);
    }

    public void AddTask(string taskName, float waitTime = 0.01f)
    {
        BasicUnitask basicUnitask = new BasicUnitask();
        basicUnitask.WaitTime = waitTime;

        tasks.Add(taskName, basicUnitask);
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
        foreach (KeyValuePair<string, BasicUnitask> task in tasks)
        {
            task.Value.source.Cancel();
            task.Value.source.Dispose();
        }

        tasks.Clear();
    }

    void WhenDisable()
    {
        foreach (KeyValuePair<string, BasicUnitask> task in tasks)
        {
            task.Value.source.Cancel();
        }
    }

    protected void WhenEnable()
    {
        foreach (KeyValuePair<string, BasicUnitask> task in tasks)
        {
            if (task.Value.source != null) task.Value.source.Dispose();
            task.Value.source = new CancellationTokenSource();
        }
    }
}
