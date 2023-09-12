using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject TutorialPanel;

    public GameObject[] tutorials;
    int tutorialIndex = 0;

    public GameObject modeSelectPanel;
    public TMP_Text modeTxt;

    string[] modes = { "Basic", "Normal", "Hard" };

    [SerializeField]
    string modeName;
    string ModeName
    {
        get { return modeName; }
        set 
        { 
            modeName = value;
            modeTxt.text = modeName;
        }
    }

    [SerializeField]
    StringSkillSlotDictionary skillSlots;

    [SerializeField]
    StringAbilitySlotDictionary abilitySlots;

    [Header("Upgrade")]
    [SerializeField]
    GameObject upgradePanel;

    [SerializeField]
    TMP_Text upgradeCommentTxt;

    [SerializeField]
    GameObject[] upgradeBtn;

    [SerializeField]
    TMP_Text[] upgradeTxt;

    [SerializeField]
    BaseSlot selectedSlot;

    [Header("Gold")]
    [SerializeField]
    TMP_Text goldTxt;

    [SerializeField]
    int totalGold;

    int TotalGold
    {
        get { return totalGold; }
        set
        {
            totalGold = value;
            goldTxt.text = totalGold.ToString();
        }
    }

    public void SelectMode(int index)
    {
        ModeName = modes[index];
        modeSelectPanel.SetActive(false);
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);
    }

    public void OnOffUpgradePanel(bool nowOn)
    {
        upgradePanel.SetActive(nowOn);
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);
    }

    public void OnOffUpgradePanel(bool nowOn, bool canUpgrade, string upgradeText)
    {
        upgradePanel.SetActive(nowOn);
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        upgradeCommentTxt.text = upgradeText;

        if (canUpgrade)
        {
            upgradeBtn[0].SetActive(true);
            upgradeTxt[0].text = "취소";

            upgradeBtn[1].SetActive(true);
            upgradeTxt[1].text = "확인";
        }
        else
        {
            upgradeBtn[0].SetActive(true);
            upgradeTxt[0].text = "확인";

            upgradeBtn[1].SetActive(false);
            upgradeTxt[1].text = "";
        }
    }

    public void ActiveUpgradePanel(BaseSlot baseSlot)
    {
        bool canUpgrade = baseSlot.CanUpgrade(totalGold, out string upgradeText);
        selectedSlot = baseSlot;

        OnOffUpgradePanel(true, canUpgrade, upgradeText);
    }

    public void Upgrade()
    {
        if (selectedSlot == null) return;

        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        OnOffUpgradePanel(false);

        TotalGold -= selectedSlot.ReturnUpgradePrice();

        selectedSlot.UpgradeTask(); // --> 업그레이드
        SaveManager.Instance.ResetData(totalGold);
        SaveManager.Instance.Save(); // 데이터 다시 저장
    }

    private void Start()
    {
        SoundManager.Instance.StopBGM();

        Screen.orientation = ScreenOrientation.Portrait;
        SaveManager.Instance.ResetSlot(ref totalGold, skillSlots, abilitySlots);
        TotalGold = totalGold; // 초기화
        ModeName = modes[0];

        // Upgrade에 Action 연결
        foreach (KeyValuePair<string, SkillSlot> skillSlot in skillSlots)
        {
            skillSlot.Value.upgradeAction += ActiveUpgradePanel;
        }
        foreach (KeyValuePair<string, AbilitySlot> abilitySlot in abilitySlots)
        {
            abilitySlot.Value.upgradeAction += ActiveUpgradePanel;
        }
    }

    private void Update() // 화면 해상도 대응
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    public void GoToPlayerScene()
    {
        SceneManager.LoadScene(ModeName);
    }

    public void OnOffModeSelect(bool nowOn)
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        modeSelectPanel.SetActive(nowOn);
    }

    public void OnOffSetting(bool nowOn)
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        settingPanel.SetActive(nowOn);
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnOffTutorial(bool nowOn)
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        TutorialPanel.SetActive(nowOn);
        if(nowOn == true)
        {
            tutorialIndex = 0;
            tutorials[0].SetActive(true);
            tutorials[1].SetActive(false);
            tutorials[2].SetActive(false);
        }
    }

    public void SwitchTutorialPanel(int tmp)
    {
        SoundManager.Instance.PlaySFX("ButtonClick", 0.3f);

        tutorialIndex += tmp;

        if (tutorialIndex == -1)
        {
            tutorialIndex = tutorials.Length - 1;
        }
        else if(tutorialIndex == tutorials.Length)
        {
            tutorialIndex = 0;
        }

        for (int i = 0; i < tutorials.Length; i++)
        {
            if(i == tutorialIndex)
                tutorials[i].gameObject.SetActive(true);
            else
                tutorials[i].gameObject.SetActive(false);
        }
    }
}
