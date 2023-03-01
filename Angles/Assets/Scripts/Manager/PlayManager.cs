using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayManager : Singleton<PlayManager>
{
    public Player player;
    public MoveJoystick moveJoy;
    public ActionJoystick actionJoy;
    public CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    protected override void Awake()
    {
        Application.targetFrameRate = 60;

        base.Awake();
        DatabaseManager DB = DatabaseManager.Instance;
        DB.PlayerData = DB.EntityDB.Player.CopyData();

        moveJoy = FindObjectOfType<MoveJoystick>();
        actionJoy = FindObjectOfType<ActionJoystick>();

        GameObject go = Resources.Load("Prefabs/Entity/Player") as GameObject;
        player = Instantiate(go).GetComponent<Player>();
        virtualCamera.Follow = player.transform;
    }
}
