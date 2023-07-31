using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

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
    public GameObject pausePanel;

    public GameObject gameClearPanel;

    [SerializeField]
    TMP_Text finalScoreTxt;

    [SerializeField]
    TMP_Text scoreTxt;

    public static PlayManager instance;
    public static PlayManager Instance { get { return instance; } }

    [SerializeField]
    Image bgmImg;

    [SerializeField]
    TMP_Text bgmTxt;

    [SerializeField]
    Image sfxImg;

    [SerializeField]
    TMP_Text sfxTxt;

    [SerializeField]
    int totalScore = 0;

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

        SoundManager.Instance.PlayBGM("BGM");
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        gameOverPanel.SetActive(true);
    }

    public void ScoreUp(int score)
    {
        totalScore += score;
        scoreTxt.text = totalScore.ToString();
    }

    public void GameClear()
    {
        finalScoreTxt.text = totalScore.ToString();
        gameClearPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnOffBGM()
    {
        if (SoundManager.Instance.BgmMasterVolume == 1)
        {
            SoundManager.Instance.BgmMasterVolume = 0;
            bgmTxt.text = "음소거";
            bgmImg.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            SoundManager.Instance.BgmMasterVolume = 1;
            bgmTxt.text = "배경음";
            bgmImg.color = new Color(1, 1, 1);
        }

    }

    public void OnOffSFX()
    {
        if(SoundManager.Instance.SfxMasterVolume == 1)
        {
            SoundManager.Instance.SfxMasterVolume = 0;
            sfxTxt.text = "음소거";
            sfxImg.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            SoundManager.Instance.SfxMasterVolume = 1;
            sfxTxt.text = "효과음";
            sfxImg.color = new Color(1, 1, 1);
        }

    }

    public void ActivePausePanel(bool nowActive)
    {
        if(nowActive)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        pausePanel.SetActive(nowActive);
    }
}
