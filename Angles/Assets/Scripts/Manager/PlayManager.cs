using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayManager : Singleton<PlayManager>
{
    Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    public CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    protected override void Awake()
    {
        Application.targetFrameRate = 60;

        base.Awake();

        GameObject go = Resources.Load("Prefabs/Entity/Player") as GameObject;
        player = Instantiate(go).GetComponent<Player>();

        player.ResetPlayerData(DatabaseManager.Instance.EntityDB.Player.CopyData());
        virtualCamera.Follow = player.transform;
    }
}
