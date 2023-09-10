using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Player player;
    float movePos = 10;

    private void Start()
    {
        player = PlayManager.Instance.Player;
    }

    public bool CanMove(float diffX, float diffY)
    {
        return diffX >= movePos || diffY >= movePos;
    }
    private void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 myPos = transform.position;

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        if (CanMove(diffX, diffY) == false) return;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        if (dirX < 0) dirX = -1;
        else if (dirX > 0) dirX = 1;

        if (dirY < 0) dirY = -1;
        else if (dirY > 0) dirY = 1;


        if (diffX >= movePos)
        {
            transform.Translate(dirX * movePos * 2, 0, 0);
        }

        if (diffY >= movePos)
        {
            transform.Translate(0, dirY * movePos * 2, 0);
        }
    }
}
