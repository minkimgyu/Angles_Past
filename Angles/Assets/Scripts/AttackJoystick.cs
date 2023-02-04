using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

public class AttackJoystick : VariableJoystick
{
    public Player player;
    Vector3 attackVec;

    public bool nowAttackReady = false;

    public bool NowAttackReady
    {
        get
        {
            return nowAttackReady;
        }
        set
        {
            nowAttackReady = value;
        }
    }

    float minJoyVal = 0.25f;
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;

    public RectTransform rush;
    Image rushImage;
    float rushRatio = 0;
    float rushPower = 5;

    UnitaskUtility fillTask = new UnitaskUtility();

    private async UniTaskVoid FillAttackPower()
    {
        fillTask.NowRunning = true;

        while (rushRatio < 1)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.01f), cancellationToken: fillTask.source.Token);
            rushRatio += 0.01f;
            rushImage.fillAmount = rushRatio;
        }

        if (rushRatio > 1) rushRatio = 1;

        fillTask.NowRunning = false;
    }

    void EndRush()
    {
        rushRatio = 0;
        rushImage.fillAmount = 0;
    }

    protected override void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        joystickType = JoystickType.Floating;
        base.Start();

        fillTask.cancelFn += EndRush;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        rushImage = rush.GetComponent<Image>();
    }

    public override void SetMode(JoystickType joystickType)
    {
        base.SetMode(joystickType);
        if (this.joystickType != JoystickType.Fixed) rush.gameObject.SetActive(false);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        float absHorizontal = Mathf.Abs(Horizontal);
        float absVertical = Mathf.Abs(Vertical);

        if (ReturnNowDrag() == true && nowAttackReady == false)
        {
            nowAttackReady = true;
            player.Animator.SetBool("NowReady", true);
            EndRush();

            FillAttackPower().Forget();
        }
        else if(ReturnNowDrag() == false && nowAttackReady == true)
        {
            nowAttackReady = false;
            player.Animator.SetBool("NowReady", false);

            fillTask.CancelTask();
        }
    }

    public bool DoubleClickCheck()
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            doubleClickedTime = -1.0f;
            return true;
        }
        else
        {
            doubleClickedTime = Time.time;
            return false;
        }
    }

    bool ReturnNowDrag()
    {
        float absHorizontal = Mathf.Abs(Horizontal);
        float absVertical = Mathf.Abs(Vertical);

        if (absHorizontal > minJoyVal || absVertical > minJoyVal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            rush.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            rush.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);

        if (DoubleClickCheck() == true)
        {
            player.Dash();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (ReturnNowDrag() == false)
        {
            if (joystickType != JoystickType.Fixed)
                rush.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
            return; // 대쉬 상태
        }

        if (nowAttackReady == true) nowAttackReady = false;

        attackVec.Set(Horizontal, Vertical, 0);

        float rushStat = rushPower * rushRatio;
        Debug.Log(rushStat);
        Debug.Log(attackVec.normalized);

        player.Attack(attackVec.normalized * rushStat);


        fillTask.CancelTask();

        if (joystickType != JoystickType.Fixed)
            rush.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    private void OnDestroy()
    {
        fillTask.WhenDestroy();
    }

    private void OnDisable()
    {
        fillTask.WhenDisable();
    }

    private void OnEnable()
    {
        fillTask.WhenEnable();
    }
}
