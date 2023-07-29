using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    float ReturnRotationAngle(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    public void RotateUsingRigidbody(Vector2 vec)
    {
        rigid.MoveRotation(ReturnRotationAngle(vec));
    }

    public void RotateUsingTransform(Vector2 vec)
    {
        transform.rotation = Quaternion.Euler(0, 0, ReturnRotationAngle(vec));
    }

    public void RotationPlayer(Vector2 vec, bool useRigid)
    {
        //bool isAttackReady = PlayManager.Instance.actionJoy.actionComponent.Mode == ActionMode.AttackReady;
        //if (isAttackReady == true)
        //{
        //    rotationVec.Set(PlayManager.Instance.actionJoy.Horizontal, PlayManager.Instance.actionJoy.Vertical);
        //}
        //else
        //{
        //    bool nowPlayerMove = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec() == Vector2.zero;
        //    if (nowPlayerMove == true) return;

        //    rotationVec.Set(rigid.velocity.x, rigid.velocity.y);
        //}

        if(useRigid) RotateUsingRigidbody(vec);
        else RotateUsingTransform(vec);
    }

    public void Stop()
    {
        rigid.velocity = Vector2.zero;
    }

    public void Move(Vector2 moveDir, Vector2 rotationDir, float speed, bool useRigid = true)
    {
        //bool nowReady = PlayManager.Instance.actionJoy.actionComponent.Mode == ActionMode.AttackReady;

        //if (nowReady == true)
        //{
        //    rigid.velocity = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec().normalized * DatabaseManager.Instance.PlayerData.ReadySpeed * DatabaseManager.Instance.PlayerData.SpeedRatio;
        //}
        //else
        //{
        //    rigid.velocity = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec().normalized * DatabaseManager.Instance.PlayerData.Speed * DatabaseManager.Instance.PlayerData.SpeedRatio;
        //}

        rigid.velocity = moveDir.normalized * speed;

        //RotationPlayer(rotationDir, useRigid);
    }

    public void Move(Vector2 moveDir, float speed, bool useRigid = true)
    {
        rigid.velocity = moveDir.normalized * speed;

       //RotationPlayer(moveDir.normalized, useRigid);
    }
}
