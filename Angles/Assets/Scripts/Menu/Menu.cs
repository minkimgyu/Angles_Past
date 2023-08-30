using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class BaseSlotData
{
    public int price;

    public int maxUpgradeCount;
    public int upgradeCount;

    public BaseSlotData(int price, int maxUpgradeCount, int upgradeCount)
    {
        this.price = price;
        this.maxUpgradeCount = maxUpgradeCount;
        this.upgradeCount = upgradeCount;
    }

    public void Initialize(int price, int maxUpgradeCount, int upgradeCount) 
    {
        this.price = price;
        this.maxUpgradeCount = maxUpgradeCount;
        this.upgradeCount = upgradeCount;
    }

    public bool CheckUpgradeCount()
    {
        return upgradeCount < maxUpgradeCount;
    }

    public bool CheckPrice(int gold)
    {
        return gold >= price;
    }
}

[System.Serializable]
public class SkillSlotData : BaseSlotData
{
    public bool isLock;
    public bool isSelect;

    public SkillSlotData(int price, int maxUpgradeCount, int upgradeCount, bool isLock, bool isSelect) : base(price, maxUpgradeCount, upgradeCount)
    {
        this.isLock = isLock;
        this.isSelect = isSelect;
    }

    public void Initialize(int price, int maxUpgradeCount, int upgradeCount, bool isLock, bool isSelect)
    {
        Initialize(price, maxUpgradeCount, upgradeCount);
        this.isLock = isLock;
        this.isSelect = isSelect;
    }
}

[System.Serializable]
public class StatSlotData : BaseSlotData
{
    public float stat;
    public float statMultiply;

    public int increaseGold;

    public StatSlotData(int price, int maxUpgradeCount, int upgradeCount, float stat, float statMultiply, int increaseGold) : base(price, maxUpgradeCount, upgradeCount)
    {
        this.stat = stat;
        this.statMultiply = statMultiply;
        this.increaseGold = increaseGold;
    }

    public void Initialize(int price, int maxUpgradeCount, int upgradeCount, float stat, float statMultiply, int increaseGold)
    {
        Initialize(price, maxUpgradeCount, upgradeCount);
        this.stat = stat;
        this.statMultiply = statMultiply;
        this.increaseGold = increaseGold;
    }
}

public abstract class Slot : MonoBehaviour
{
    public string buyText;

    public virtual void CantUpgrade() { }

    public void Initialize() { }

    abstract public BaseSlotData ReturnData();

    abstract public void Upgrade();
}

public class Menu : MonoBehaviour
{
    public EntityDB entityDB;
    public Image cheatImg;
    public TMP_Text cheatTxt;

    public GameObject settingPanel;
    public GameObject TutorialPanel;

    public GameObject[] tutorials;
    int tutorialIndex = 0;

    public TMP_Text modeTxt;
    public GameObject modeSelectPanel;

    [SerializeField]
    string nowSelectedMode;

    [SerializeField]
    string startMode = "Basic Mode";

    [SerializeField]
    int gold;

    int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldTxt.text = value.ToString() + "G";
        }
    }

    [SerializeField]
    TMP_Text goldTxt;

    [Header("Ask")]
    [SerializeField]
    GameObject askPanel;

    [SerializeField]
    TMP_Text askTxt;

    [Header("Return")]
    [SerializeField]
    GameObject returnPanel;

    [SerializeField]
    TMP_Text returnTxt;

    int saveGold = 0;
    Slot selectSlot;

    [Header("DataStorage")]
    public StringStatSlotDictionary statSlots; // key�� ã�Ƽ� �Ѱ��ֱ�
    public StringSkillSlotDictionary skillSlots; 

    private void Start()
    {
        SaveManager.instance.Initialize(ref gold, statSlots, skillSlots);
        Gold = gold;

        if (!entityDB.Player.Immortality)
        {
            cheatTxt.text = "���� ����";
            cheatImg.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        }
        else
        {
            cheatTxt.text = "���� ����";
            cheatImg.color = new Color(1f, 1f, 1f, 1f);
        }

        nowSelectedMode = startMode;
        modeTxt.text = nowSelectedMode;
    }
       
    public void Buy(Slot slot)
    {
        BaseSlotData data = slot.ReturnData();

        if(!data.CheckPrice(Gold) || !data.CheckUpgradeCount()) // ���׷��̵尡 �Ұ����� ���
        {
            slot.CantUpgrade();
            SaveManager.instance.LoadDataInSlot(gold, statSlots, skillSlots); // ���� ������Ʈ�� �����Ѵٸ� ���� �����͸� �ҷ��ͼ� �����Ŵ
        }

        if (!data.CheckPrice(Gold) && data.CheckUpgradeCount()) // ���׷��̵�� ���������� Gold�� ���ڶ� ���
        {
            OnOffPanel(returnPanel, returnTxt, true, "���� ���� ���� Gold�� �����մϴ�.");
            return;
        }

        if (!data.CheckUpgradeCount()) // �ִ� ���׷��̵��� ���
        {
            return;
        }

        selectSlot = slot;
        saveGold = data.price;

        OnOffPanel(askPanel, askTxt, true, "���� " + saveGold + "G�� ����Ͽ�" + "\n" + slot.buyText);
    }

    public void OffReturnPanel()
    {
        OnOffPanel(returnPanel, returnTxt, false);
    }

    void OnOffPanel(GameObject panel, TMP_Text txt, bool isOpen, string text = "")
    {
        panel.SetActive(isOpen);
        txt.text = text;
    }

    public void NowBuy(bool isYes)
    {
        if(isYes)
        {
            print(saveGold);

            Gold -= saveGold;
            selectSlot.Upgrade(); // do

            SaveManager.instance.LoadDataInSlot(gold, statSlots, skillSlots); // ���� ������Ʈ�� �����Ѵٸ� ���� �����͸� �ҷ��ͼ� �����Ŵ
        }
        else
        {
            saveGold = 0;
            selectSlot = null;
        }

        OnOffPanel(askPanel, askTxt, false);
    }

    public void OnOffModeSelectPanel(bool nowOn)
    {
        modeSelectPanel.SetActive(nowOn);
    }

    public void SelectMode(string mode)
    {
        modeTxt.text = mode;
        nowSelectedMode = mode;
        modeSelectPanel.SetActive(false);
    }

    public void GoToPlayerScene()
    {
        SceneManager.LoadScene(nowSelectedMode);
    }

    public void OnOffSetting(bool nowOn)
    {
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

    public void MakePlayerImmo()
    {
        if(!entityDB.Player.Immortality)
        {
            cheatTxt.text = "���� ����";
            cheatImg.color = new Color(1f, 1f, 1f, 1f);
            entityDB.Player.Immortality = true;
        }
        else
        {
            cheatTxt.text = "���� ����";
            cheatImg.color = new Color(0.1f, 0.1f, 0.1f, 1f);
            entityDB.Player.Immortality = false;
        }
    }
}
