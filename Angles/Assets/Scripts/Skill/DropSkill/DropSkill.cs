using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSkill : MonoBehaviour
{
    [SerializeField]
    string skillName;

    public string ReturnSkill()
    {
        gameObject.SetActive(false);
        return skillName;
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}