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
    Vector3 attackVec;
    public Player player;

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

    public Action<Vector2> AttackAction;
    public Action<Vector2> DashAction;

    public AttackUIComponent attackUIComponent;

    protected override void Start()
    {
        player = PlayManager.Instance.player;
        joystickType = JoystickType.Floating;
        base.Start();
    }

    public override void SetMode(JoystickType joystickType)
    {
        base.SetMode(joystickType);
        if (this.joystickType != JoystickType.Fixed) attackUIComponent.Rush.gameObject.SetActive(false);
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

            attackUIComponent.FillAttackPower().Forget();
        }
        else if(ReturnNowDrag() == false && nowAttackReady == true)
        {
            nowAttackReady = false;
            player.Animator.SetBool("NowReady", false);

            attackUIComponent.fillTask.CancelTask();
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
            attackUIComponent.Rush.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            attackUIComponent.Rush.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);

        if (DoubleClickCheck() == true)
        {
            DashAction(); //--> 밖에서 따로 연결
            player.Dash();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (ReturnNowDrag() == false)
        {
            if (joystickType != JoystickType.Fixed)
                attackUIComponent.Rush.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
            return; // 대쉬 상태
        }

        if (nowAttackReady == true) nowAttackReady = false;

        attackVec.Set(Horizontal, Vertical, 0);





        float rushStat = rushPower * rushRatio;
        Debug.Log(rushStat);
        Debug.Log(attackVec.normalized);



        //AttackAction(); --> 밖에서 따로 연결

        player.Attack(attackVec.normalized * rushStat);


        attackUIComponent.ReturnRushStat() * atta;

        if (joystickType != JoystickType.Fixed)
            rush.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}
