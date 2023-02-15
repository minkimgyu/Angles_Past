using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float levelUpTime;
    public string spawnEntityName;
    public int spawnIndex;
    public int spawnCount;

    public bool NowCanSpawn(float time)
    {
        if (time > levelUpTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public int level = 0;
    public float time;

    public List<SpawnData> spawnDatas = new List<SpawnData>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (spawnDatas.Count - 1 < level) return;

        bool nowCanSpawn = spawnDatas[level].NowCanSpawn(time);
        if (nowCanSpawn == true)
        {
            Spawn(spawnDatas[level].spawnIndex, spawnDatas[level].spawnCount, spawnDatas[level].spawnEntityName);
            // 여기에 스폰 데이터 넣기
            level += 1;
        }
    }

    public void Spawn(int spawnIndex, int spawnCount, string spawnEntityName)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = spawnPoints[spawnIndex].position;
            GameObject entity = ObjectPooler.SpawnFromPool(spawnEntityName);
            entity.GetComponent<Entity>().Init(ReturnEntityData("Player"));
        }
    }

    EntityData ReturnEntityData(string name)
    {
        EntityData[] entityDatas = DatabaseManager.Instance.EntityDB.entityDatas;
        for (int i = 0; i < entityDatas.Length; i++)
        {
            if(entityDatas[i].Name == name)
            {
                return entityDatas[i];
            }
        }

        return null;
    }
}
