using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class GameStorageData
{
    public int totalGold;

    public int maxSelectCount = 3;

    public StringSkillSlotDataDictionary skillSlotDatas;

    public StringAbilitySlotDataDictionary abilitySlotDatas;
}


public class SaveManager : Singleton<SaveManager>
{
    [SerializeField]
    GameStorageData gameStorageData;

    [SerializeField]
    GameStorageData initialGameStorageData;

    string filePath;

    public List<string> ReturnSkills()
    {
        List<string> skillNames = new List<string>();

        foreach (KeyValuePair<string, SkillSlotData> skillSlotData in gameStorageData.skillSlotDatas)
        {
            if(skillSlotData.Value.nowUse == true)
            {
                skillNames.Add(skillSlotData.Key + "Item");
            }
        }

        return skillNames;
    }

    public void ResetGold(int additionalGold)
    {
        gameStorageData.totalGold += additionalGold;
        Save();
    }

    public bool CanSelectSkill()
    {
        if (SelectedSkillCount() < gameStorageData.maxSelectCount) return true;
        else return false;
    }

    int SelectedSkillCount()
    {
        int useCount = 0;

        foreach (KeyValuePair<string, SkillSlotData> skillSlot in gameStorageData.skillSlotDatas)
        {
            if (skillSlot.Value.nowUse) useCount++;
        }

        return useCount;
    }

    void Start()
    {
        // 전체 아이템 리스트 불러오기
        filePath = Application.persistentDataPath + "/GameData.txt";
        print(filePath);
        Load();
    }

    // 슬롯 데이터를 재설정 
    public void ResetSlot(ref int totalGold, StringSkillSlotDictionary skillSlots, StringAbilitySlotDictionary abilitySlots)
    {
        totalGold = gameStorageData.totalGold;

        foreach (KeyValuePair<string, SkillSlot> skillSlot in skillSlots)
        {
            string key = skillSlot.Key;
            if (gameStorageData.skillSlotDatas.ContainsKey(key) == false) return;

            skillSlot.Value.Initialize(gameStorageData.skillSlotDatas[key]);
        }

        foreach (KeyValuePair<string, AbilitySlot> abilitySlot in abilitySlots)
        {
            string key = abilitySlot.Key;
            if (gameStorageData.abilitySlotDatas.ContainsKey(key) == false) return;

            abilitySlot.Value.Initialize(gameStorageData.abilitySlotDatas[key]);
        }
    }

    public void ResetData(int totalGold, int maxSelectCount = 3)
    {
        gameStorageData.totalGold = totalGold;
        gameStorageData.maxSelectCount = maxSelectCount;
    }

    public void Save()
    {
        string jdata = JsonConvert.SerializeObject(gameStorageData);//Convert to json
        File.WriteAllText(filePath, jdata);
    }

    public void Save(GameStorageData gameStorageData)
    {
        string jdata = JsonConvert.SerializeObject(gameStorageData);//Convert to json
        File.WriteAllText(filePath, jdata);
    }

    void Load()
    {
        if (!File.Exists(filePath)) 
        {
            Save(initialGameStorageData); // 파일이 존재하지 않을 경우 초기 데이터 저장
        } 

        string jdata = File.ReadAllText(filePath);
        gameStorageData = JsonConvert.DeserializeObject<GameStorageData>(jdata);
    }
}
