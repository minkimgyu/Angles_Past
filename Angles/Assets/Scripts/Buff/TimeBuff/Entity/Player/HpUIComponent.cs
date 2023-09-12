using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUIComponent : MonoBehaviour
{
    [SerializeField]
    GameObject[] hp;

    [SerializeField]
    PlayerActionEventSO eventSO;

    private void Awake()
    {
        eventSO.OnActionRequested += ResetHpUI;
    }

    private void OnDisable()
    {
        eventSO.OnActionRequested -= ResetHpUI;
    }

    public void ResetHpUI(float hpPoint)
    {
        for (int i = 0; i < hp.Length; i++)
        {
            hp[i].SetActive(false);
        }


        for (int i = 0; i < hpPoint; i++)
        {
            hp[i].SetActive(true);
        }
    }
}
