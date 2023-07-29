using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionEnemy : MonoBehaviour
{
    Player player;
    SpawnAssistant spawnAssistant;

    private void Start()
    {
        player = PlayManager.Instance.Player;
        spawnAssistant = GameObject.FindWithTag("Player").GetComponentInChildren<SpawnAssistant>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy") == true)
        {
            if (col.name.Contains("Heptagon") || col.name.Contains("Octagon")) return; // Heptagon, Octagon Á¦¿Ü

            Reposition(col);
        }
    }

    Vector3 FindClosestPoint(List<Transform> points)
    {
        float distance = 0;
        int loadIndex = 0;

        for (int i = 0; i < points.Count; i++)
        {
            if(i == 0)
            {
                distance = Vector3.Distance(points[i].position, transform.position);
                loadIndex = i;
            }
            else
            {
                float nowIndexDistance = Vector3.Distance(points[i].position, transform.position);
                if (distance > nowIndexDistance)
                { 
                    distance = nowIndexDistance;
                    loadIndex = i;
                }
            }
        }

        return points[loadIndex].position;
    }

    void Reposition(Collider2D col)
    {
        List<Transform> closePoints = spawnAssistant.FindSpawnPoint();
        if (closePoints.Count <= 0) return;

        col.transform.position = FindClosestPoint(closePoints);
        col.GetComponent<MoveComponent>().Stop();
    }

    private void Update()
    {
        if (player == null) return;
        transform.position = player.transform.position;
    }
}
