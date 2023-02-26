using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemyComponent : MonoBehaviour
{
    public GameObject go;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") == true && col.gameObject != go)
        {
            DatabaseManager.Instance.SpeedRatio -= 0.2f;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") == true && col.gameObject != go)
        {
            DatabaseManager.Instance.SpeedRatio += 0.2f;
        }
    }
}
