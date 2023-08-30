using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class AdditionalPlayerData
{
    public AdditionalPlayerData(int life, float weight, float speed)
    {
        this.life = life;
        this.weight = weight;
        this.speed = speed;
    }

    public int life;
    public float weight;
    public float speed;

    public void Initialize(PlayerData data)
    {
        data.Hp += life;
        data.Weight += weight;

        data.DashThrust += speed * 5f;
        data.RushThrust += speed * 5f;
        data.Speed.OriginValue += speed;
        data.ReadySpeed.OriginValue += speed;
    }
}

[System.Serializable]
public class GameDataStorage
{
    public int gold;

    [SerializeField]
    public StringSkillSlotDataDictionary skillSlotDatas; // isSelect �Ǿ��ִ� ��ų�� ��� ����

    [SerializeField]
    public StringStatSlotDataDictionary statSlotDatas; // ���⼭ �÷��̾� ���� ����

    public void Clear()
    {
        gold = 0;
        skillSlotDatas.Clear();
        statSlotDatas.Clear();
    }

    public void Initialize()
    {
        gold = 300;
        skillSlotDatas.Add("Shield", new SkillSlotData(0, 1, 1, false, true));
        skillSlotDatas.Add("Ghost", new SkillSlotData(0, 1, 1, false, true));
        skillSlotDatas.Add("Impact", new SkillSlotData(500, 1, 0, true, false));
        skillSlotDatas.Add("Knockback", new SkillSlotData(500, 1, 0, true, false));
        skillSlotDatas.Add("Blade", new SkillSlotData(500, 1, 0, true, false));

        skillSlotDatas.Add("StickyBomb", new SkillSlotData(1000, 1, 0, true, false));
        skillSlotDatas.Add("SpawnBall", new SkillSlotData(1000, 1, 0, true, false));
        skillSlotDatas.Add("GravitationalField", new SkillSlotData(1000, 1, 0, true, false));
        skillSlotDatas.Add("Boomerang", new SkillSlotData(1500, 1, 0, true, false));
        skillSlotDatas.Add("Tornado", new SkillSlotData(1500, 1, 0, true, false));

        statSlotDatas.Add("Life", new StatSlotData(100, 5, 1, 0, 1, 100));
        statSlotDatas.Add("Mass", new StatSlotData(100, 5, 1, 0, 1, 100));
        statSlotDatas.Add("Speed", new StatSlotData(100, 5, 1, 0, 1, 100));
    }
}

public class SaveManager : Singleton<SaveManager>
{
    // ���⼭ �����͸� �ҷ����� �����Ŵ
    // ���Կ��� �����͸� �ٲ�ٸ� �ٽ� �����Ű�� ������� ����
    [SerializeField]
    GameDataStorage gameDataStorage;

    string filePath;

    public string[] ReturnDropSkills()
    {
        List<string> skillNames = new List<string>();

        foreach (KeyValuePair<string, SkillSlotData> data in gameDataStorage.skillSlotDatas)
        {
            if(data.Value.isSelect == true)
            {
                skillNames.Add(data.Key + "Item");
            }
        }

        return skillNames.ToArray();
    }

    public void ResetPlayerData(PlayerData playerData)
    {
        AdditionalPlayerData additionalPlayerData = new AdditionalPlayerData(
            (int)gameDataStorage.statSlotDatas["Life"].stat,
            gameDataStorage.statSlotDatas["Mass"].stat,
            gameDataStorage.statSlotDatas["Speed"].stat);

        additionalPlayerData.Initialize(playerData);
    }

    public void SaveTotalScore(int score)
    {
        gameDataStorage.gold += score;
        Save();
    }

    public void Initialize(ref int gold, StringStatSlotDictionary statSlots, StringSkillSlotDictionary skillSlots) // �� �Լ��� ������ �ݿ�
    {
        gold = gameDataStorage.gold;

        // iter�� ��ȸ��Ű�鼭 key�� ã�Ƽ� �ʱ�ȭ�ϱ�

        foreach (KeyValuePair<string, StatSlot> slot in statSlots)
        {
            string tmpKey = slot.Key;

            // ������ key�� �ش��ϴ� �������� key�� ���� ��� continue
            if (gameDataStorage.statSlotDatas[tmpKey] == null) continue;

            slot.Value.Initialize(gameDataStorage.statSlotDatas[tmpKey]);
        }

        foreach (KeyValuePair<string, SkillSlot> slot in skillSlots)
        {
            string tmpKey = slot.Key;

            // ������ key�� �ش��ϴ� �������� key�� ���� ��� continue
            if (gameDataStorage.skillSlotDatas[tmpKey] == null) continue;

            slot.Value.Initialize(gameDataStorage.skillSlotDatas[tmpKey]);
        }
    }

    public void LoadDataInSlot(int gold, StringStatSlotDictionary statSlots, StringSkillSlotDictionary skillSlots) // �� �Լ��� ������ �ݿ�
    {
        gameDataStorage.gold = gold;

        // iter�� ��ȸ��Ű�鼭 key�� ã�Ƽ� �ʱ�ȭ�ϱ�

        foreach (KeyValuePair<string, StatSlot> slot in statSlots)
        {
            string tmpKey = slot.Key;

            // ������ key�� �ش��ϴ� �������� key�� ���� ��� continue
            if (gameDataStorage.statSlotDatas[tmpKey] == null) continue;

            gameDataStorage.statSlotDatas[tmpKey] = new StatSlotData(slot.Value.statSlotData.price, slot.Value.statSlotData.maxUpgradeCount,
                        slot.Value.statSlotData.upgradeCount, slot.Value.statSlotData.stat, slot.Value.statSlotData.statMultiply,
                        slot.Value.statSlotData.increaseGold);
        }

        foreach (KeyValuePair<string, SkillSlot> slot in skillSlots)
        {
            string tmpKey = slot.Key;

            // ������ key�� �ش��ϴ� �������� key�� ���� ��� continue
            if (gameDataStorage.skillSlotDatas[tmpKey] == null) continue;


            gameDataStorage.skillSlotDatas[tmpKey] = new SkillSlotData(slot.Value.skillSlotData.price, slot.Value.skillSlotData.maxUpgradeCount,
                        slot.Value.skillSlotData.upgradeCount, slot.Value.skillSlotData.isLock, slot.Value.skillSlotData.isSelect);
        }

        Save(); // ���� ���̺� ���ֱ�
    }

    protected override void Awake()
    {
        base.Awake();
        filePath = Application.persistentDataPath + "/MyData.txt";

        print(filePath);

        if(!File.Exists(filePath))
        {
            gameDataStorage.Clear();

            gameDataStorage.Initialize(); // �⺻ ������ ���̺� ���ֱ�
            Save();
        }

        Load();
    }

    public void Save()
    {
        string jdata = JsonConvert.SerializeObject(gameDataStorage);
        File.WriteAllText(filePath, jdata);
    }

    public void Load()
    {
        string jdata = File.ReadAllText(filePath);
        gameDataStorage = JsonConvert.DeserializeObject<GameDataStorage>(jdata);
    }
}