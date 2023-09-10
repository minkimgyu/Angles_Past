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

    public GameObject pausePanel;
    public GameObject gameClearPanel;

    [SerializeField]
    TMP_Text gameClearTxt;

    [SerializeField]
    TMP_Text finalScoreTxt;

    [SerializeField]
    TMP_Text finalGoldTxt;

    [SerializeField]
    TMP_Text scoreTxt;

    static PlayManager instance;
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
    int goldCount = 0;

    int maxGoldCount = 0;

    [SerializeField]
    bool m_gameClear = false;
    public bool GameClearCheck { get { return m_gameClear; }}

    [SerializeField]
    CanvasScaler playCanvasScaler;

    [SerializeField]
    RectTransform move;

    [SerializeField]
    RectTransform attack;

    bool nowPortrait = true;

    // Start is called before the first frame update
    protected void Awake()
    { 
        instance = this; // 이부분은 ObjectPooler와 유사한 싱글톤으로 만들어주기
        Application.targetFrameRate = 60;
    }

    private void Update() // 화면 해상도 대응
    {
        if (Screen.orientation == ScreenOrientation.Portrait && nowPortrait == false)
        {
            // 여기에 화면이 돌아가면 변경될 점을 적어주자
            move.sizeDelta = new Vector2(540, 1920);
            attack.sizeDelta = new Vector2(540, 1920);
            playCanvasScaler.referenceResolution = new Vector2(1080, 1920);
            virtualCamera.m_Lens.OrthographicSize = 12;
            nowPortrait = true;
        }
        else if ((Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) && nowPortrait == true)
        {
            move.sizeDelta = new Vector2(960, 1080);
            attack.sizeDelta = new Vector2(960, 1080);
            playCanvasScaler.referenceResolution = new Vector2(1920, 1080);
            virtualCamera.m_Lens.OrthographicSize = 9;
            nowPortrait = false;
        }
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("BGM");
    }

    public void GameOver()
    {
        gameClearTxt.text = "Game Over";
        finalScoreTxt.text = totalScore.ToString();
        finalGoldTxt.text = ((int)totalScore / 2).ToString();

        SaveManager.Instance.ResetGold((int)totalScore / 2);
        gameClearPanel.SetActive(true);
    }

    public void GoldUp(int score)
    {
        ScoreUp(score);
        goldCount += 1;
        //player.AddAdditionalStat(goldCount / maxGoldCount);
    }

    public void ScoreUp(int score)
    {
        totalScore += score;
        scoreTxt.text = totalScore.ToString();
    }

    public void GameClear()
    {
        m_gameClear = true;
        gameClearTxt.text = "Game Clear";
        finalScoreTxt.text = totalScore.ToString();
        finalGoldTxt.text = ((int)totalScore / 2).ToString();

        SaveManager.Instance.ResetGold((int)totalScore / 2);
        gameClearPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("PlayScene");
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
