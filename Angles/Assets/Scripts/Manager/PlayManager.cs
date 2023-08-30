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
        set 
        { 
            player = value;
            virtualCamera.Follow = value.transform;
        }
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

    [SerializeField]
    bool m_gameClear = false;
    public bool GameClearCheck { get { return m_gameClear; } }

    bool nowPortrait = true;

    //public string[] dropSkills = { "GhostItem", "BarrierItem", "BladeItem", "KnockbackItem", "ShockwaveItem", "SpawnBallItem", "SpawnGravityBallItem", "StickyBombItem" };

    public string[] dropSkills;

    public RectTransform move;
    public RectTransform rush;
    public CanvasScaler canvasScaler;

    // Start is called before the first frame update
    protected void Awake()
    { 
        instance = this;
        Application.targetFrameRate = 60;
<<<<<<< Updated upstream
=======
        

        //base.Awake();
        GameObject go = Resources.Load("Prefabs/Entity/Player") as GameObject;
        player = Instantiate(go).GetComponent<Player>();
>>>>>>> Stashed changes
    }

    private void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait && !nowPortrait)
        {
            nowPortrait = true;
            ChangePortrait(nowPortrait);
        }
        else if((Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) && nowPortrait)
        {
            nowPortrait = false;
            ChangePortrait(nowPortrait);
        }
    }

    void ChangePortrait(bool nowPortrait)
    {
        if(nowPortrait)
        {
            move.sizeDelta = new Vector2(540, 1920);
            rush.sizeDelta = new Vector2(540, 1920);
            virtualCamera.m_Lens.OrthographicSize = 12;
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
        }
        else
        {
            move.sizeDelta = new Vector2(960, 1080);
            rush.sizeDelta = new Vector2(960, 1080);
            virtualCamera.m_Lens.OrthographicSize = 8;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
        }
    }

    private void Start()
    {
<<<<<<< Updated upstream
=======
        dropSkills = SaveManager.instance.ReturnDropSkills();

        player.InitData();
        virtualCamera.Follow = player.transform;

>>>>>>> Stashed changes
        SoundManager.Instance.PlayBGM("BGM");
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ScoreUp(int score)
    {
        totalScore += score;
        scoreTxt.text = totalScore.ToString();
    }

    public void GameClear()
    {
        m_gameClear = true;
        finalScoreTxt.text = totalScore.ToString();

        SaveManager.instance.SaveTotalScore(totalScore);
        gameClearPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
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
