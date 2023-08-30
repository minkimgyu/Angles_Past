using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField]
    int point = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(transform.position, "GetItem", 0.3f);
            PlayManager.Instance.ScoreUp(point);

            collision.GetComponent<Player>().UpMassAndScale();
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
