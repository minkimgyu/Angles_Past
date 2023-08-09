using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}

public class SaveManager : MonoBehaviour
{
    string filePath;

    void Start()
    {
        // 전체 아이템 리스트 불러오기
        filePath = Application.persistentDataPath + "/SaveData/MyData.txt";

        print(filePath);
        Load();
    }

    void Save()
    {
        //string jdata = JsonUtility.ToJson(new Serialization<string>(MyItemList));
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        //string code = System.Convert.ToBase64String(bytes);

        //File.WriteAllText(filePath, code);
    }

    void Load()
    {
        if (!File.Exists(filePath)) {  } // 파일이 존재하지 않을 경우

        string code = File.ReadAllText(filePath);

        //byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        //MyItemList = JsonUtility.FromJson<Serialization<string>>(jdata).target;

        //TabClick(curType);
    }

}
