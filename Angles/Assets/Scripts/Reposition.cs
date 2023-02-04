using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Player player;

    public Transform[] spawnPoints;

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform.CompareTag("Enemy") == true)
        {
            if(col.enabled == true)
            {
                Vector3 tempVec = FindClosePoint();

                float xPos = Random.Range(-2, 2);
                float yPos = Random.Range(-2, 2);

                Vector3 spawnPos = new Vector3(tempVec.x + xPos, tempVec.y + yPos, 0);
                col.transform.position = spawnPos;
            }
        }
    }

    public Vector3 FindClosePoint()
    {
        float distance = -1;
        int index = 0;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i == 0)
            {
                distance = Vector2.Distance(spawnPoints[i].transform.position, player.transform.position);
            }
            else
            {
                float newDistance = Vector2.Distance(spawnPoints[i].transform.position, player.transform.position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    index = i;
                    Debug.Log(i);
                }
            }
        }



        return spawnPoints[index].position;
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
