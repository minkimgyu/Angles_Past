using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public class BasicController
//{
//    protected Vector2 mainVec = Vector2.zero;
//    public virtual Vector2 MainVec
//    {
//        get { return mainVec; }
//    }

//    Vector2 loadVec = Vector2.zero;
//    public Vector2 LoadVec
//    {
//        get { return loadVec; }
//        set { loadVec = value; }
//    }

//    public bool CheckCancelAttack(float offset)
//    {
//        float xOffset = Mathf.Abs(loadVec.x - MainVec.x);
//        float yOffset = Mathf.Abs(loadVec.y - MainVec.y);

//        if (xOffset >= offset || yOffset >= offset)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public void SetLoadVec()
//    {
//        loadVec.Set(mainVec.x, mainVec.y);
//    }

//    public void SetMainVec(float x, float y)
//    {
//        mainVec.Set(x, y);
//    }
//}

//public class KeyController : BasicController
//{
    
//}

//public class JoyController : BasicController
//{
//    MoveJoystick moveJoystick;

//    public JoyController(MoveJoystick moveJoy)
//    {
//        moveJoystick = moveJoy;
//    }

//    public override Vector2 MainVec
//    {
//        get {
//            SetMainVec(moveJoystick.Horizontal, moveJoystick.Vertical);
//            return mainVec; 
//        }
//    }
//}


public class MoveInputComponent : MonoBehaviour
{
    //JoyController joyController;
    //KeyController keyController;

    //MoveJoystick moveJoy;

    //private void Awake()
    //{
    //    moveJoy = GetComponent<MoveJoystick>();

    //    joyController = new JoyController(moveJoy);
    //    keyController = new KeyController();
    //}

    //public Vector2 ReturnMoveVec()
    //{
    //    if(joyController.MainVec != Vector2.zero)
    //    {
    //        return joyController.MainVec;
    //    }
    //    else
    //    {
    //        return keyController.MainVec;
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    SubUpdate();
    //}

    //void SubUpdate()
    //{
    //    keyController.SetMainVec(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    //}

    //public bool NowCancelAttack()
    //{
    //    float offset = DatabaseManager.Instance.PlayerData.AttackCancelOffset;

    //    if (joyController.CheckCancelAttack(offset) == true || keyController.CheckCancelAttack(offset) == true)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    ////public bool NowCanDash()
    ////{
    ////    if (joyController.CheckNowUseDash() == true || keyController.CheckNowUseDash() == true)
    ////    {
    ////        return true;
    ////    }
    ////    else
    ////    {
    ////        return false;
    ////    }
    ////}

    //public void SetLoadVec()
    //{
    //    joyController.SetLoadVec();
    //    keyController.SetLoadVec(); 
    //}
}
