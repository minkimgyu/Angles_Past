using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackJoystick : VariableJoystick
{
    public Player play;
    Vector3 attackVec;

    protected override void Start()
    {
        play = GameObject.FindWithTag("Player").GetComponent<Player>();
        joystickType = JoystickType.Floating;
        base.Start();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(Horizontal);
        Debug.Log(Vertical);

        attackVec.Set(Horizontal, Vertical, 0);
        play.Attack(attackVec);

        base.OnPointerUp(eventData);
    }
}
