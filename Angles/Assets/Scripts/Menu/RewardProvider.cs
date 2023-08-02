using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

abstract public class RewardTimer : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text timeTxt;

    [SerializeField]
    protected float sToWait;

    protected bool isRunning = false;

    ulong lastTimeClicked;

    protected float msToWait { get { return sToWait * 1000; } }

    private void Update()
    {
        ShowTime();
    }

    public void ShowTime()
    {
        if (!isRunning) return;

        float secondsLeft = ReturnLeftSeconds();
        if (Ready(secondsLeft) == true) return;

        ResetTimeTxt(secondsLeft);
    }

    public virtual void ResetTimeTxt(float secondsLeft)
    {
        string r = "";
        //HOURS
        r += ((int)secondsLeft / 3600).ToString() + ":";
        secondsLeft += ((int)secondsLeft / 3600) * 3600;
        //MINUTES
        r += ((int)secondsLeft / 60).ToString("00") + ":";
        //SECONDS
        r += (secondsLeft % 60).ToString("00");

        timeTxt.text = r;
    }

    public virtual void ResetTimer()
    {
        if (isRunning == false)
        {
            lastTimeClicked = (ulong)DateTime.Now.Ticks;
            isRunning = true;
            WhenReset();
        }
    }

    public virtual bool Ready(float secondsLeft)
    {
        if (secondsLeft < 0)
        {
            isRunning = false;
            
            WhenFinished();
            return true;
        }

        return false;
    }

    public abstract void WhenFinished();

    public abstract void WhenReset();

    float ReturnLeftSeconds()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - lastTimeClicked);
        ulong m = diff / TimeSpan.TicksPerMillisecond;
        return (float)(msToWait - m) / 1000.0f;
    }
}


public class RewardProvider : RewardTimer
{
    [SerializeField]
    protected Button btn;

    private void Start()
    {
        ResetTimer();
    }

    public override void WhenReset()
    {
        btn.interactable = false;
    }

    public override void WhenFinished()
    {
        btn.interactable = true;
        timeTxt.text = "Finish!";
    }
}
