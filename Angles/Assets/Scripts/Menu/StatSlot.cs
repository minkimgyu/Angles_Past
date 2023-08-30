using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatSlot : Slot
{
    public StatSlotData statSlotData;

    [SerializeField]
    TMP_Text maxUpgradeTxt;

    [SerializeField]
    TMP_Text upgradeTxt;

    private void Start()
    {
        buyText = "업그레이드 하시겠습니까?";
    }

    public void Initialize(StatSlotData statSlotData)
    {
        this.statSlotData = statSlotData;
        maxUpgradeTxt.text = this.statSlotData.maxUpgradeCount.ToString();
        upgradeTxt.text = this.statSlotData.upgradeCount.ToString();
    }

    public override void Upgrade()
    {
        statSlotData.price += statSlotData.increaseGold;
        statSlotData.upgradeCount++;

        upgradeTxt.text = statSlotData.upgradeCount.ToString();

        statSlotData.stat = statSlotData.statMultiply * statSlotData.upgradeCount;
    }

    public override BaseSlotData ReturnData()
    {
        return statSlotData;
    }
}
