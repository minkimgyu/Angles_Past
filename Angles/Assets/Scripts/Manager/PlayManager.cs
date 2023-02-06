using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    public Player player;
    public MoveJoystick moveJoy;
    public ActionJoystick actionJoy;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;

        player = FindObjectOfType<Player>();
        moveJoy = FindObjectOfType<MoveJoystick>();
        actionJoy = FindObjectOfType<ActionJoystick>();
    }
}
