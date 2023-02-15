using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEnemyCheck : MonoBehaviour
{
    public GameObject go;
    FollowComponent followComponent;
    List<GameObject> closeEnemys = new List<GameObject>();

    private void Start()
    {
        followComponent = go.GetComponent<FollowComponent>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && col.gameObject != go)
        {
            followComponent.count += 1;
            ResetRatio(followComponent.count);
            closeEnemys.Add(col.gameObject);
        }
    }
    
    void ResetRatio(int count)
    {
        if(count == 0)
        {
            followComponent.ratio = 1f;

        }
        else
        {
            followComponent.ratio = 1f / count;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && col.gameObject != go)
        {
            followComponent.count -= 1;
            ResetRatio(followComponent.count);
            closeEnemys.Add(col.gameObject);
        }
    }
}
