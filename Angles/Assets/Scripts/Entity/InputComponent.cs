using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveController : BasicController
{
    Vector2 loadVec = Vector2.zero;
    public Vector2 LoadVec
    {
        get { return loadVec; }
        set { loadVec = value; }
    }

    public bool CheckCancelAttack(float offset)
    {
        float xOffset = Mathf.Abs(loadVec.x - mainVec.x);
        float yOffset = Mathf.Abs(loadVec.y - mainVec.y);

        if(xOffset >= offset || yOffset >= offset)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckNowUseSkill(float ratio)
    {
        return Mathf.Abs(mainVec.x) <= ratio && Mathf.Abs(mainVec.y) <= ratio;
    }

    public void SetLoadVec()
    {
        loadVec.Set(mainVec.x, mainVec.y);
    }
}

public class AttackController : BasicController
{
    
}

public class BasicController
{
    protected Vector2 mainVec = Vector2.zero;
    public Vector2 MainVec
    {
        get { return MainVec; }
    }

    public void SetMainVec(float x, float y)
    {
        mainVec.Set(x, y);
    }
}

public class InputComponent : MonoBehaviour
{
    Player player;

    float offset = 0.35f;
    float ratio = 0.1f; // 이하면 대쉬, 이상이면 러쉬로 적용됨

    public MoveController moveJoystick = new MoveController();
    public MoveController keyboard = new MoveController();
    public AttackController attackJoystick = new AttackController();

    MoveJoystick moveJoy;
    AttackJoystick attackJoy;

    Dash dash;
    Attack attack;

    private void Awake()
    {
        player = GetComponent<Player>();
        player.UpdateAction += SubUpdate;

        moveJoy = FindObjectOfType<MoveJoystick>();
        attackJoy = FindObjectOfType<AttackJoystick>();

        dash = GetComponent<Dash>();
        attack = GetComponent<Attack>();

        attackJoy.DashAction += dash.PlayDash;

        //attackJoy.AttackAction += Attack;
        //attackJoy.DashAction += Dash;
        // 연결은 여기서 진행
    }

    void SubUpdate()
    {
        keyboard.SetMainVec(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveJoystick.SetMainVec(moveJoy.Horizontal, moveJoy.Vertical);
        attackJoystick.SetMainVec(attackJoy.Horizontal, attackJoy.Vertical);
    }

    public bool NowCancelAttack()
    {
        if(moveJoystick.CheckCancelAttack(offset) == true || keyboard.CheckCancelAttack(offset) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanDashOrAttack()
    {
        if (moveJoystick.CheckNowUseSkill(ratio) == true && keyboard.CheckNowUseSkill(ratio) == true)
        {
            return true; // 대쉬 가능
        }
        else
        {
            return false; // 러쉬 가능
        }
    }

    public void SetLoadVec()
    {
        moveJoystick.SetLoadVec(); 
        keyboard.SetLoadVec(); 
    }
}
