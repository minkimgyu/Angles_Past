using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

[System.Serializable]
public class OctaPoint
{
    public GameObject octaGo;
    public Transform tr;

    public Vector3 Pos { get { return tr.position; } }

    public bool CheckAleadySpawn()
    {
        if (octaGo == null) return true;
        else
        {
            if(octaGo.activeSelf == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void SpawnToPoint(GameObject go)
    {
        octaGo = null;
        octaGo = go;
    }
}

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public OctaPoint[] octagonSpawnPoints;

    public int level = 0;
    public float time;

    public SpawnDatas data;

    SpawnAssistant spawnAssistant;

    EntityFactory entityFactory;

    [SerializeField]
    TextMeshProUGUI timeTxt;

    private void Awake()
    {
        entityFactory = GetComponent<EntityFactory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player player = (Player)entityFactory.OrderEntity("Player");
        spawnAssistant = player.GetComponentInChildren<SpawnAssistant>();

        PlayManager.Instance.virtualCamera.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAssistant == null) return;

        time += Time.deltaTime;
        timeTxt.text = string.Format("{0:F0}", time);

        if (data.SpawnDataCollection.Count - 1 < level) return;

        bool nowCanSpawn = data.SpawnDataCollection[level].NowCanSpawn(time);
        if (nowCanSpawn == true)
        {
            if(data.SpawnDataCollection[level].spawnEntityName.Contains("Octagon"))
            {
                int index = ReturnCorrectPointIndex();

                if(index == -1) return;

                Vector3 pos = octagonSpawnPoints[index].Pos;
                Spawn(pos, data.SpawnDataCollection[level].spawnEntityName, index);
            }
            else
            {
                List<Transform> spawnAssistantPoints = spawnAssistant.FindSpawnPoint();
                if (spawnAssistantPoints.Count > 0)
                {
                    Vector3 pos = spawnAssistantPoints[Random.Range(0, spawnAssistantPoints.Count)].position;
                    Spawn(pos, data.SpawnDataCollection[level].spawnCount, data.SpawnDataCollection[level].spawnEntityName);
                }
                else
                {
                    Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    Spawn(pos, data.SpawnDataCollection[level].spawnCount, data.SpawnDataCollection[level].spawnEntityName);
                }
            }

            // 여기에 스폰 데이터 넣기
            level += 1;
        }
    }

    bool CheckHasEmptyPoint()
    {
        for (int i = 0; i < octagonSpawnPoints.Length; i++)
        {
            if (octagonSpawnPoints[i].CheckAleadySpawn() == true)
            {
                return true;
            }
        }

        return false;
    }

    int ReturnCorrectPointIndex()
    {
        int index = -1;

        if (CheckHasEmptyPoint() == false) return index;

        while (true)
        {
            int tmp = Random.Range(0, octagonSpawnPoints.Length);

            bool canSpawn = octagonSpawnPoints[tmp].CheckAleadySpawn();
            if(canSpawn == true)
            {
                index = tmp;
                break;
            }
        }

        return index;
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
            Entity entity = entityFactory.OrderEntity(spawnEntityName);
            Vector3 resetPos = SpawnNotOverlap(pos, loadSpawnPos);
            entity.transform.position = resetPos;
        }
    }

    public void Spawn(Vector3 pos, string spawnEntityName, int indexOfPoint)
    {
        Entity entity = ObjectPooler.SpawnFromPool<Entity>(spawnEntityName);

        octagonSpawnPoints[indexOfPoint].SpawnToPoint(entity.gameObject);
        entity.transform.position = pos;
    }
}
