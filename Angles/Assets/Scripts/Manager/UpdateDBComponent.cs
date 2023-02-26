using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.Reflection;

public class UpdateDBComponent : UnitaskUtility
{
    string URL = "https://docs.google.com/spreadsheets/d/1O7SEwFq29jxrSKmd9YspbBP--aJ27_BjbWKm873P-zk/export?format=tsv&range=";
    string range = "A1:A1";

    public List<DataDictionary> dataDictionaryList;

    public Dictionary<string, string> nameAndRange = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Awake()
    {
        DatabaseManager.Instance.EntityDB.ResetData(); // 초기화
        LoadInitRange().Forget();
    }
        
    public async UniTaskVoid LoadInitRange()
    {
        nowRunning = true;

        string url = URL + range;

        UnityWebRequest www = UnityWebRequest.Get(url);
        await www.SendWebRequest();
        string initData = URL + www.downloadHandler.text; // 초기 범위 확인


        UnityWebRequest www1 = UnityWebRequest.Get(initData);
        await www1.SendWebRequest();
        SetEntityDB(www1.downloadHandler.text); // 추가 범위 체크

        nowRunning = false;
    }

    void SetEntityDB(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        Dictionary<string, string> nameAndRange = new Dictionary<string, string>();

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            string name = column[0];
            string range = column[1];

            nameAndRange.Add(name, range);
            print(name + " : " + range);

            LoadData(name, range).Forget();
            // 여기에 데이터 채워주는 스크립트 추가
        }
    }

    public async UniTaskVoid LoadData(string dataName, string range)
    {
        nowRunning = true;

        string url = URL + range;

        UnityWebRequest www = UnityWebRequest.Get(url);
        await www.SendWebRequest();
        string tsv = www.downloadHandler.text; // 초기 범위 확인
        AddData(dataName, tsv);

        nowRunning = false;
    }

    void AddData(string dataName, string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 1; i < columnSize; i++)
        {
            Type type = Type.GetType(dataName + "Data");
            object objType = Activator.CreateInstance(type);

            for (int j = 0; j < rowSize; j++)
            {
                string[] column = row[j].Split('\t');
                string key = column[0];
                string value = column[i];
                ResetTypeOfData(objType, key, value);
            }

            ResetDB(dataName, objType);
        }
    }

    void ResetDB(string dataName, object data)
    {
        if (dataName == "Player") DatabaseManager.Instance.EntityDB.Player = data as PlayerData;
       
        else if (dataName == "Enemy") DatabaseManager.Instance.EntityDB.Enemy.Add(data as EnemyData);

        else if (dataName == "Skill") DatabaseManager.Instance.EntityDB.Skill.Add(data as SkillData);
    }

    void ResetTypeOfData(object TypeData, string propertyName, string value)
    {
        PropertyInfo property = TypeData.GetType().GetProperty(propertyName);
        object tempValue;

        if (property.PropertyType == typeof(int))
        {
            tempValue = int.Parse(value);
        }
        else if (property.PropertyType == typeof(float))
        {
            tempValue = float.Parse(value);
        }
        else if (property.PropertyType == typeof(SkillName))
        {
            tempValue = (SkillName)Enum.Parse(typeof(SkillName), value);
        }
        else if (property.PropertyType == typeof(SkillUseType))
        {
            tempValue = (SkillUseType)Enum.Parse(typeof(SkillUseType), value);
        }
        else if (property.PropertyType == typeof(EntityTag[]))
        {
            tempValue = SplitHitTargets(value);
        }
        else
        {
            tempValue = value;
        }

        property.SetValue(TypeData, tempValue);
    }

    EntityTag[] SplitHitTargets(string value)
    {
        string[] row = value.Split('+');
        EntityTag[] entityTag = new EntityTag[row.Length];
        for (int i = 0; i < row.Length; i++)
        {
            entityTag[i] = (EntityTag)Enum.Parse(typeof(EntityTag), row[i]);
        }

        return entityTag;
    }
}
