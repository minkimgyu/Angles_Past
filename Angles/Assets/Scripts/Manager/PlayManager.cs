using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayManager : MonoBehaviour //Singleton<PlayManager>
{
    Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    public CinemachineVirtualCamera virtualCamera;

    public GameObject gameOverPanel;

    public static PlayManager instance;
    public static PlayManager Instance { get { return instance; } }

    // Start is called before the first frame update
    protected void Awake()
    { 
        instance = this;

        Application.targetFrameRate = 60;

        //base.Awake();
        GameObject go = Resources.Load("Prefabs/Entity/Player") as GameObject;
        player = Instantiate(go).GetComponent<Player>();
    }

    private void Start()
    {
        player.InitData();
        virtualCamera.Follow = player.transform;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
