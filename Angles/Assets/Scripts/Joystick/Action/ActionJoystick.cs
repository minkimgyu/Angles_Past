using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

public class ActionJoystick : VariableJoystick
{
    public RectTransform rush;

    float doubleClickedTime = -1.0f;
    float interval = 0.25f;

    public Action DashAction;
    public Action AttackAction;
    public Action AttackReadyAction;

    public Vector2 MainVec
    {
        get { return new Vector2(Horizontal, Vertical); }
    }

    protected override void Start()
    {
        joystickType = JoystickType.Floating;
        base.Start();
    }

    public override void SetMode(JoystickType joystickType)
    {
        base.SetMode(joystickType);
        if (this.joystickType != JoystickType.Fixed) rush.gameObject.SetActive(false);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        AttackReady(Horizontal, Vertical); // 공격 준비 단계
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            rush.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            rush.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);

        Dash();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Attack(Horizontal, Vertical);

        if (joystickType != JoystickType.Fixed) rush.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
    public void Dash()
    {
        if (NowDoubleClick() == true && DashAction != null) DashAction();
    }

    public void AttackReady(float horizontal, float vertical)
    {
        if (AttackReadyAction != null) AttackReadyAction();
    }

    public void Attack(float horizontal, float vertical)
    {
        if (AttackAction != null) AttackAction();
    }

    bool NowDoubleClick()
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            doubleClickedTime = -1.0f; // 초기화
            return true;
        }
        else
        {
            doubleClickedTime = Time.time;
            return false;
        }
    }
}
