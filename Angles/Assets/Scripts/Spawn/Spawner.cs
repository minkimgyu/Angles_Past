using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float levelUpTime;
    public string spawnEntityName;
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
            //List<Transform> spawnPoints = PlayManager.Instance.player.SpawnAssistant.FindSpawnPoint();
            //if(spawnPoints.Count > 0)
            //{
            //    Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            //    Spawn(pos, spawnDatas[level].spawnCount, spawnDatas[level].spawnEntityName);
            //}
           
            // 여기에 스폰 데이터 넣기
            level += 1;
        }
    }

    Vector3 ReturnRandomPos(Vector2 pos) 
    {
        int xDistance = Random.Range(0, 4);
        int yDistance = Random.Range(0, 4);

        return new Vector3(pos.x + xDistance, pos.y + yDistance);
    }

    Vector3 SpawnNotOverlap(Vector3 pos, List<Vector3> loadPos)
    {
        bool findNotOverlapVar = true;
        Vector3 changePos = Vector3.zero;

        if(loadPos.Count > 0)
        {
            while (findNotOverlapVar == true)
            {
                changePos = ReturnRandomPos(pos);
                findNotOverlapVar = loadPos.Contains(changePos); // 포함 안하면 false로 반환 --> 루프 벗어남
            }
        }
        else
        {
            changePos = ReturnRandomPos(pos);
        }

        loadPos.Add(changePos);
        return changePos;
    }

    public void Spawn(Vector3 pos, int spawnCount, string spawnEntityName)
    {
        List<Vector3> loadSpawnPos = new List<Vector3>();

        for (int i = 0; i < spawnCount; i++)
        {
            EnemyData enemyData = DatabaseManager.Instance.ReturnEnemyData(spawnEntityName);
            GameObject entity = ObjectPooler.SpawnFromPool(spawnEntityName);
            //entity.GetComponent<Enemy>().Init(enemyData);

            Vector3 resetPos = SpawnNotOverlap(pos, loadSpawnPos);
            entity.transform.position = resetPos;
        }
    }
}
