using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionComponent : MonoBehaviour // state가 idle, AttackReady, Attack 3종류가 있음
{
    //RushUIComponent _rushUIComponent;

    //public RushUIComponent RushUIComponent
    //{
    //    get { return _rushUIComponent; }
    //}

    //float minJoyVal = 0.25f;
    //float interval = 0.25f;
    //float doubleClickedTime = -1.0f;

    //public Action<Vector2> AttackAction;
    //public Action<Vector2> DashAction;
    //public Action<bool> AniAction;

    //Vector2 rushVec = Vector2.zero;
    //public Vector2 RushVec
    //{
    //    get { return rushVec; }
    //}

    //private void Awake()
    //{
    //    _rushUIComponent = GetComponent<RushUIComponent>();
    //}

    //public bool CheckOverMinValue(float horizontal, float vertical)
    //{
    //    float absHorizontal = Mathf.Abs(horizontal);
    //    float absVertical = Mathf.Abs(vertical);

    //    if (absHorizontal > minJoyVal || absVertical > minJoyVal) // 공격인 경우
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //} --> 플레이어로 이동

    //public void Dash()
    //{
    //    MoveJoystick moveJoy = PlayManager.Instance.moveJoy;

    //    if (NowDoubleClick() == true && DatabaseManager.Instance.PlayerData.CanUseDash() == true)
    //    {
    //        if (DashAction == null) return;
    //        DashAction(moveJoy.moveInputComponent.ReturnMoveVec().normalized);
    //    }
    //}

    //public void AttackReady(float horizontal, float vertical)
    //{
    //    if (CheckOverMinValue(horizontal, vertical))
    //    {
    //        if (AniAction == null) return;
    //        AniAction(true);
    //    }
    //    else
    //    {
    //        if (AniAction == null) return;
    //        AniAction(false);
    //    }
    //}

    //public void Attack(float horizontal, float vertical)
    //{
    //    if (CheckOverMinValue(horizontal, vertical))
    //    {
    //        if (AttackAction != null) return;
    //        AttackAction(RushVec);
    //    }
    //}

    //bool NowDoubleClick()
    //{
    //    if ((Time.time - doubleClickedTime) < interval)
    //    {
    //        doubleClickedTime = -1.0f; // 초기화
    //        return true;
    //    }
    //    else
    //    {
    //        doubleClickedTime = Time.time;
    //        return false;
    //    }
    //}
}
