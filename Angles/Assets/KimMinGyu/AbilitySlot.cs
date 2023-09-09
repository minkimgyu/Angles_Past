using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class AbilitySlotData : BaseSlotData
{
    public int upgradeCount;
    public int maxUpgradeCount;

    public float priceUpRatio;

    public AbilitySlotData(int price, string dataName, int upgradeCount, int maxUpgradeCount) : base(price)
    {
        this.dataName = dataName;
        this.upgradeCount = upgradeCount;
        this.maxUpgradeCount = maxUpgradeCount;
    }

    public void PriceUp() => price += (int)(price * priceUpRatio);
}

public class AbilitySlot : BaseSlot, ISlot<AbilitySlotData>
{
    public TMP_Text abilityNameTxt;
    public TMP_Text nowlevelTxt;
    public TMP_Text maxlevelTxt;

    public AbilitySlotData data;

    protected override void Awake()
    {
        base.Awake();
        UpgradeText.Add(UpgradeCondition.NowMaxUpgrade, "최대 업그레이드 상태입니다.");
    }

    public void Initialize(AbilitySlotData data)
    {
        this.data = data;

        abilityNameTxt.text = data.dataName;
        nowlevelTxt.text = data.upgradeCount.ToString();
        maxlevelTxt.text = data.maxUpgradeCount.ToString();
    }

    public override void UpgradeTask()
    {
        data.upgradeCount++;
        nowlevelTxt.text = data.upgradeCount.ToString();
        data.PriceUp();
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
        return data.upgradeCount < data.maxUpgradeCount;
    }

    protected override int ReturnPrice()
    {
        return data.price;
    }
}
