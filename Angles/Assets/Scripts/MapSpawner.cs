using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public Transform parent;
    public List<GameObject> mapPrefab = new List<GameObject>();
    public List<GameObject> additionalMap = new List<GameObject>();

    // 기본적으로 1개의 맵 + 플레이어 위치에 따라서 추가로 1개의 맵이 존재해야함
    public Transform basicTr;

    Player player;
    float movePos = 10;

    public int mapXIndex = 0;
    public int mapYIndex = 0;

    private void Start()
    {
        player = PlayManager.Instance.player;

        for (int i = 0; i < mapPrefab.Count; i++)
        {
            GameObject go = Instantiate(mapPrefab[i]);
            go.transform.SetParent(parent);

            additionalMap.Add(go);
            additionalMap[i].SetActive(false);
        }
    }

    public bool CanMove(float diffX, float diffY)
    {
        return diffX >= movePos || diffY >= movePos;
    }

    private void Update()
    {
        Spawn();
    }

    void ShowNextMap(Vector3 pos)
    {
        for (int i = 0; i < additionalMap.Count; i++)
        {
            additionalMap[i].SetActive(false);
        }

        if (mapXIndex == 0 && mapYIndex == 0) return;

        int random = Random.Range(0, additionalMap.Count);
        additionalMap[random].SetActive(true);
        additionalMap[random].transform.position = pos;
    }

    bool CheckPos(int temp)
    {
        return temp % 40 == 10;
    }

    void Spawn()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 myPos = basicTr.position;

        float originX = Mathf.Abs(playerPos.x - transform.position.x);
        float originY = Mathf.Abs(playerPos.y - transform.position.y);

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        if (CanMove(diffX, diffY) == false) return;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        if (dirX < 0) dirX = -1;
        else if (dirX > 0) dirX = 1;

        if (dirY < 0) dirY = -1;
        else if (dirY > 0) dirY = 1;


        if (diffX >= movePos)
        {
            Vector3 pos = new Vector3(dirX * movePos, 0, 0);

            basicTr.Translate(pos);

            if (dirX > 0) mapXIndex += 1;
            else if (dirX < 0) mapXIndex -= 1;

            if (CheckPos((int)originX) == true)
            {
                ShowNextMap(basicTr.position + pos);
            }
        }

        if (diffY >= movePos)
        {
            Vector3 pos = new Vector3(0, dirY * movePos, 0);

            basicTr.Translate(pos);

            if (dirY > 0) mapYIndex += 1;
            else if (dirY < 0) mapYIndex -= 1;

            if (CheckPos((int)originY) == true)
            {
                ShowNextMap(basicTr.position + pos);
            }
        }
    }
}