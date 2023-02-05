using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    public Player player;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;

        player = FindObjectOfType<Player>();
        //moveJoy = FindObjectOfType<MoveJoystick>();
        //attackJoy = FindObjectOfType<AttackJoystick>();
    }
}
