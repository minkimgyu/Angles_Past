using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class BasicController
{
    protected Vector2 mainVec = Vector2.zero;
    public virtual Vector2 MainVec
    {
        get { return mainVec; }
    }

    public void SetMainVec(float x, float y)
    {
        mainVec.Set(x, y);
    }
}

public class KeyController : BasicController
{
    public override Vector2 MainVec
    {
        get
        {
            SetMainVec(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            return mainVec;
        }
    }
}

public class JoyController : BasicController
{
    MoveJoystick moveJoystick;

    public JoyController(MoveJoystick moveJoy)
    {
        moveJoystick = moveJoy;
    }

    public override Vector2 MainVec
    {
        get
        {
            SetMainVec(moveJoystick.Horizontal, moveJoystick.Vertical);
            return mainVec;
        }
    }
}

public class MoveJoystick : VariableJoystick
{
    JoyController joyController;
    KeyController keyController;

    private void Awake()
    {
        joystickType = JoystickType.Floating;
        joyController = new JoyController(this);
        keyController = new KeyController();
    }

    public Vector2 ReturnMoveVec()
    {
        if (joyController.MainVec != Vector2.zero)
        {
            return joyController.MainVec.normalized;
        }
        else
        {
            return keyController.MainVec.normalized;
        }
    }
}
