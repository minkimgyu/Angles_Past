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

    int maxGoldCount = 100;

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
        instance = this; // �̺κ��� ObjectPooler�� ������ �̱������� ������ֱ�
        Application.targetFrameRate = 60;
    }

    private void Update() // ȭ�� �ػ� ����
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait && nowPortrait == false)
        {
            Screen.orientation = ScreenOrientation.Portrait;

            // ���⿡ ȭ���� ���ư��� ����� ���� ��������
            move.sizeDelta = new Vector2(540, 1920);
            attack.sizeDelta = new Vector2(540, 1920);
            playCanvasScaler.referenceResolution = new Vector2(1080, 1920);
            virtualCamera.m_Lens.OrthographicSize = 12;
            nowPortrait = true;
        }
        else if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) && nowPortrait == true)
        {

            if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
            else if(Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            {
                Screen.orientation = ScreenOrientation.LandscapeRight;
            }

            move.sizeDelta = new Vector2(960, 1080);
            attack.sizeDelta = new Vector2(960, 1080);
            playCanvasScaler.referenceResolution = new Vector2(1920, 1080);
            virtualCamera.m_Lens.OrthographicSize = 6;
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
        finalGoldTxt.text = totalScore.ToString();

        SaveManager.Instance.ResetGold(totalScore);
        gameClearPanel.SetActive(true);
    }

    public void GoldUp(int score)
    {
        totalScore += score;
        scoreTxt.text = totalScore.ToString();
        goldCount += 1;
        if(goldCount > maxGoldCount && scoreTxt.color != Color.red)
        {
            scoreTxt.color = Color.red;
        }

        player.AddAdditionalStat();
    }

    public void GameClear()
    {
        m_gameClear = true;
        gameClearTxt.text = "Game Clear";
        finalGoldTxt.text = totalScore.ToString();

        SaveManager.Instance.ResetGold(totalScore);
        gameClearPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        SceneManager.LoadScene("Menu");

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void OnOffBGM()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        if (SoundManager.Instance.BgmMasterVolume == 1)
        {
            SoundManager.Instance.BgmMasterVolume = 0;
            bgmTxt.text = "���Ұ�";
            bgmImg.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            SoundManager.Instance.BgmMasterVolume = 1;
            bgmTxt.text = "�����";
            bgmImg.color = new Color(1, 1, 1);
        }

    }

    public void OnOffSFX()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        if (SoundManager.Instance.SfxMasterVolume == 1)
        {
            SoundManager.Instance.SfxMasterVolume = 0;
            sfxTxt.text = "���Ұ�";
            sfxImg.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            SoundManager.Instance.SfxMasterVolume = 1;
            sfxTxt.text = "ȿ����";
            sfxImg.color = new Color(1, 1, 1);
        }

    }

    public void ActivePausePanel(bool nowActive)
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        if (nowActive)
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
