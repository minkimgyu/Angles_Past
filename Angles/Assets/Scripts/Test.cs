using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        transform.position = PlayManager.Instance.player.transform.position;
    }
}
