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
    Vector3 attackVec;

    public ActionUIComponent attackUIComponent;
    public ActionComponent actionComponent;

    public RectTransform rush;

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
        actionComponent.SetActionMode(Horizontal, Vertical);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            rush.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            rush.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);

        actionComponent.CheckCanDash();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        actionComponent.CheckCanAttack(Horizontal, Vertical);

        if (joystickType != JoystickType.Fixed) rush.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}
