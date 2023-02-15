using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Networking;

public class UpdateDBComponent : UnitaskUtility
{
    string URL = "https://docs.google.com/spreadsheets/d/1O7SEwFq29jxrSKmd9YspbBP--aJ27_BjbWKm873P-zk/export?format=tsv&range=";
    public string range = "A2:D3";

    // Start is called before the first frame update
    void Awake()
    {
        URL += range;

        print(URL);
        LoadData().Forget();
    }

    public async UniTaskVoid LoadData()
    {
        NowRunning = true;
        UnityWebRequest www = UnityWebRequest.Get(URL);
        await www.SendWebRequest();
        SetEntityDB(www.downloadHandler.text);

        // 이후 시작
        NowRunning = false;
    }

    void SetEntityDB(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
            {
                EntityData tempData = DatabaseManager.Instance.EntityDB.entityDatas[i];

                tempData.Name = column[0];
                tempData.Hp = float.Parse(column[1]);
                tempData.Damage = float.Parse(column[2]);
                tempData.MoveSpeed = float.Parse(column[3]);
            }
        }
    }
}
