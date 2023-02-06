using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    float angle = 0;
    Player player;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        player.fixedUpdateAction += SubUpdate;

        rigid = GetComponent<Rigidbody2D>();
    }

    void RotateUsingVelocity()
    {
        float tempAngle = angle;

        angle = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg;

        if (angle == tempAngle) return;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void SubUpdate()
    {
        RotateUsingVelocity();

        if (player.PlayerMode != PlayerMode.Idle) return;

        Move();
    }

    public void Move()
    {
        bool nowReady = PlayManager.Instance.actionJoy.actionComponent.Mode == ActionMode.AttackReady;
        
        if (nowReady == true)
        {
            rigid.velocity = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec() * DatabaseManager.Instance.ReadySpeed;
        }
        else
        {
            rigid.velocity = PlayManager.Instance.moveJoy.moveInputComponent.ReturnMoveVec() * DatabaseManager.Instance.MoveSpeed;
        }
    }

    private void OnDisable()
    {
        player.fixedUpdateAction -= SubUpdate;
    }
}
