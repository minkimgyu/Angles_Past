using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    Player player;
    Rigidbody2D rigid;

    Vector2 rotationVec = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        player.fixedUpdateAction += SubUpdate;

        rigid = GetComponent<Rigidbody2D>();
    }

    float CheckCanRotate(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    public void RotateUsingVelocity(Vector2 vec)
    {
        //float lerpAngle = Mathf.Lerp(rigid.rotation, CheckCanRotate(vec), Time.deltaTime * 3);
        rigid.MoveRotation(CheckCanRotate(vec));
    }

    public void RotateUsingTransform(Vector2 vec)
    {
        //float lerpAngle = Mathf.Lerp(rigid.rotation, CheckCanRotate(vec), Time.deltaTime * 3);
        transform.rotation = Quaternion.Euler(0, 0, CheckCanRotate(vec));
    }

    void RotationPlayer()
    {
        bool isAttackReady = PlayManager.Instance.actionJoy.actionComponent.Mode == ActionMode.AttackReady;
        if (isAttackReady == true)
        {
            rotationVec.Set(PlayManager.Instance.actionJoy.Horizontal, PlayManager.Instance.actionJoy.Vertical);
        }
        else
        {
            bool nowPlayerMove = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec() == Vector2.zero;
            if (nowPlayerMove == true) return;

            rotationVec.Set(rigid.velocity.x, rigid.velocity.y);
        }

        RotateUsingVelocity(rotationVec.normalized);
    }

    void SubUpdate()
    {
        if (player.PlayerMode != ActionMode.Idle) return;
        Move();
        RotationPlayer();
    }

    public void Move()
    {
        bool nowReady = PlayManager.Instance.actionJoy.actionComponent.Mode == ActionMode.AttackReady;
        
        if (nowReady == true)
        {
            rigid.velocity = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec().normalized * DatabaseManager.Instance.ReadySpeed;
        }
        else
        {
            rigid.velocity = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec().normalized * DatabaseManager.Instance.MoveSpeed;
        }
    }

    private void OnDisable()
    {
        player.fixedUpdateAction -= SubUpdate;
    }
}
