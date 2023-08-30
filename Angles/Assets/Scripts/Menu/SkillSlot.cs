using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillSlot : Slot
{
    public SkillSlotData skillSlotData;

    public GameObject lockUI;

    [SerializeField]
    TMP_Text lockGoldTxt;

    public GameObject selectUI;

    private void Start()
    {
        buyText = "구매하시겠습니까?";
    }

    public override void CantUpgrade()
    {
        SelectSkill();
    }

    void SelectSkill()
    {
        if (skillSlotData.isLock == true) return;

        skillSlotData.isSelect = !skillSlotData.isSelect;
        selectUI.SetActive(skillSlotData.isSelect);
    }

    public void Initialize(SkillSlotData skillSlotData)
    {
        this.skillSlotData = skillSlotData;

        lockUI.SetActive(skillSlotData.isLock);
        selectUI.SetActive(skillSlotData.isSelect);

        lockGoldTxt.gameObject.SetActive(skillSlotData.isLock);
        lockGoldTxt.text = skillSlotData.price.ToString() + "G";
    }

    public override void Upgrade()
    {
        skillSlotData.upgradeCount++;
        skillSlotData.isLock = false;
        lockUI.SetActive(skillSlotData.isLock);
    }

    public override BaseSlotData ReturnData()
    {
        return skillSlotData;
    }
}
