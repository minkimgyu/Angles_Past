using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

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
        //// 전체 아이템 리스트 불러오기
        //filePath = Application.persistentDataPath + "/MyData.txt";
        //if (!File.Exists(filePath)) return; // 파일 없으면

        //print(filePath);

        //float[] scalePerTick = { 1, 2, 3 };
        //EntityTag[] entity = { EntityTag.Enemy };
        //Dictionary<EffectCondition, EffectData> effectDatas = new Dictionary<EffectCondition, EffectData>() 
        //{ { EffectCondition.HitSurfaceEffect, new EffectData() } };


        //Dictionary<EffectCondition, SoundData> soundDatas = new Dictionary<EffectCondition, SoundData>()
        //{ { EffectCondition.HitSurfaceEffect, new SoundData() } };

        //Dictionary<string, BaseSkill> objectList1 = new Dictionary<string, BaseSkill>()
        //{
        //    { "stickybomb", new CasterCircleRangeAttack("stickybomb", true, 0, 0, 2, 10, scalePerTick, entity, 5, 5, effectDatas, soundDatas, "ready") },
        //};

        //string data = JsonConvert.SerializeObject(objectList1, new JsonSerializerSettings
        //{
        //    TypeNameHandling = TypeNameHandling.All
        //});


        //File.WriteAllText(filePath, data);
        //print(data);

        //string jdata = File.ReadAllText(filePath);

        //Dictionary<string, BaseBuff> objectList2 = JsonConvert.DeserializeObject<Dictionary<string, BaseBuff>>(jdata, new JsonSerializerSettings
        //{
        //    TypeNameHandling = TypeNameHandling.Auto
        //});

        //print(objectList2["base"]);


        //TimeBuff1 timeBuff1 = (TimeBuff1)objectList2["time"];

        //print(timeBuff1.maxTickTime);
        //print(timeBuff1.duration);



        //print(filePath);

        //File.WriteAllText(filePath, "하이하이");

        //Load();
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

        string jdata = File.ReadAllText(filePath);

        List<BaseBuff> baseBuffs = JsonUtility.FromJson<Serialization<BaseBuff>>(jdata).target;

        //byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        //MyItemList = JsonUtility.FromJson<Serialization<string>>(jdata).target;

        //TabClick(curType);
    }

}
