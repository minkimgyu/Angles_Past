using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class MoveJoystick : VariableJoystick
{
    public MoveInputComponent moveInputComponent;
    public DashUIComponent dashUIComponent;

    public Action fixedUpdateAction;

    private void FixedUpdate()
    {
        fixedUpdateAction();
    }
}
