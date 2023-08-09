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
        // ��ü ������ ����Ʈ �ҷ�����
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
        if (!File.Exists(filePath)) {  } // ������ �������� ���� ���

        string code = File.ReadAllText(filePath);

        //byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        //MyItemList = JsonUtility.FromJson<Serialization<string>>(jdata).target;

        //TabClick(curType);
    }

}
