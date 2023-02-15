using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionComponent : MonoBehaviour
{
    ActionUIComponent uIComponent;

    float minJoyVal = 0.25f;
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;

    public Action<Vector2, ForceMode2D> attackAction;
    public Action<Vector2> dashAction;
    public Action<bool> aniAction;

    Vector2 rushVec = Vector2.zero;
    Vector2 dashVec = Vector2.zero;

    private void Awake()
    {
        uIComponent = GetComponent<ActionUIComponent>();
    }

    ActionMode mode = ActionMode.Idle;

    public ActionMode Mode
    {
        get { return mode; }
    }

    public bool CheckOverMinValue(float horizontal, float vertical)
    {
        float absHorizontal = Mathf.Abs(horizontal);
        float absVertical = Mathf.Abs(vertical);

        if (absHorizontal > minJoyVal || absVertical > minJoyVal) // 공격인 경우
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetActionMode(float horizontal, float vertical)
    {
        if(CheckOverMinValue(horizontal, vertical) == true)
        {
            if (NowModeCheck(ActionMode.AttackReady) == false)
            {
                mode = ActionMode.AttackReady;
                uIComponent.FillAttackPower().Forget();

                aniAction(true);
            }
        }
        else
        {
            if (NowModeCheck(ActionMode.Idle) == false)
            {
                mode = ActionMode.Idle;
                uIComponent.CancelTask();
                Debug.Log("NowCancel");

                aniAction(false);
            }
        }
    }

    public bool NowModeCheck(ActionMode nowChangeMode) { return mode == nowChangeMode; }

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

    public void CheckCanDash()
    {
        MoveJoystick moveJoy = PlayManager.Instance.moveJoy;

        if (NowDoubleClick() == true && DatabaseManager.Instance.CanUseDash() == true && moveJoy.moveInputComponent.NowCanDash() == true)
        {
            dashAction(moveJoy.moveInputComponent.ReturnMoveVec().normalized); // 대쉬 값을 받아와서 진행
            moveJoy.dashUIComponent.ReFillDashRatio();
        }
    }

    public void CheckCanAttack(float horizontal, float vertical)
    {
        if(CheckOverMinValue(horizontal, vertical) == true)
        {
            rushVec.Set(horizontal, vertical);

            attackAction(-rushVec.normalized * uIComponent.RushRatio * DatabaseManager.Instance.AttackThrust, ForceMode2D.Impulse); // Ratio와 역방향 고려
            mode = ActionMode.Idle;
            uIComponent.CancelTask();
        }
    }
}
