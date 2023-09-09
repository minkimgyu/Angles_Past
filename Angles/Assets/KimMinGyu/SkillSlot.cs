using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public interface ISlot<T>
{
    public void Initialize(T value);
}

abstract public class BaseSlot : MonoBehaviour
{
    public Dictionary<UpgradeCondition, string> UpgradeText = new Dictionary<UpgradeCondition, string>();

    public enum UpgradeCondition
    {
        NotEnoughGold,
        NowMaxUpgrade,
        CanUpgrade
    }

    protected virtual void Awake()
    {
        UpgradeText.Add(UpgradeCondition.NotEnoughGold, "��尡 �����մϴ�.");
        UpgradeText.Add(UpgradeCondition.CanUpgrade, "��带 ����Ͽ� �ر��Ͻðڽ��ϱ�?");
    }

    public Action<BaseSlot> upgradeAction;

    protected abstract int ReturnPrice();

    public abstract int ReturnUpgradePrice();

    public bool CanUpgrade(int totalGold, out string upgradeText) // --> �̰� �����ϰ� ��ųʸ��� ������ �����ؼ� �ؽ�Ʈ ��������
    {
        bool notMaxUpgrade = NotMaxUpgrade();
        bool nowEnoughGold = NowEnoughGold(totalGold);

        if (nowEnoughGold == false)
        {
            upgradeText = UpgradeText[UpgradeCondition.NotEnoughGold];
        }
        else if (notMaxUpgrade == false)
        {
            upgradeText = UpgradeText[UpgradeCondition.NowMaxUpgrade];
        }
        else
        {
            upgradeText = ReturnPrice().ToString() + UpgradeText[UpgradeCondition.CanUpgrade];
        }

        return notMaxUpgrade && nowEnoughGold;
    }

    public abstract bool NowEnoughGold(int totalGold);

    public abstract bool NotMaxUpgrade();

    public virtual void DoUpgrade() => upgradeAction(this);
    public abstract void UpgradeTask();
}

abstract public class BaseSlotData
{
    public int price;
    public string dataName;

    public BaseSlotData(int price)
    {
        this.price = price;
    }
}

[System.Serializable]
public class SkillSlotData : BaseSlotData
{
    public string path;

    public bool nowUse;
    public bool nowLock;

    public SkillSlotData(int price, bool nowUse, bool nowLock) : base(price)
    {
        this.nowUse = nowUse;
        this.nowLock = nowLock;
    }
}

public class SkillSlot : BaseSlot, ISlot<SkillSlotData>
{
    public TMP_Text nameTxt;
    public Image skillImage;
    public Image useImage;
    public GameObject lockGO;

    public GameObject priceGO;
    public TMP_Text priceTxt;

    public Color useColor = new Color(0, 1, 0, 1);
    public Color normalColor = new Color(1, 1, 1, 1);

    public SkillSlotData data;

    bool NowUse
    {
        get { return data.nowUse; }
        set
        {
            data.nowUse = value;

            if (data.nowUse) useImage.color = useColor;
            else useImage.color = normalColor;
        }
    }

    bool NowLock
    {
        get { return data.nowLock; }
        set
        {
            data.nowLock = value;
            lockGO.SetActive(data.nowLock);

            priceGO.gameObject.SetActive(data.nowLock);
        }
    }

    public void Initialize(SkillSlotData data)
    {
        this.data = data;

        skillImage.sprite = Resources.Load<Sprite>(data.path);
        nameTxt.text = data.dataName;
        priceTxt.text = data.price.ToString();

        this.NowUse = data.nowUse;
        this.NowLock = data.nowLock;
    }
    
    public override void UpgradeTask()
    {
        NowLock = false;
    }

    public override void DoUpgrade()
    {
        if(NowLock == false)
        {
            if (NowUse == false && SaveManager.Instance.CanSelectSkill())
            {
                NowUse = true;
            }
            else if (NowUse == true)
            {
                NowUse = false;
            }

            return;
        }

        base.DoUpgrade();
    }

    public override int ReturnUpgradePrice()
    {
        return data.price;
    }

    public override bool NowEnoughGold(int totalGold)
    {
        return data.price <= totalGold;
    }

    public override bool NotMaxUpgrade()
    {
        return NowLock == true;
    }

    protected override int ReturnPrice()
    {
        return data.price;
    }
}
