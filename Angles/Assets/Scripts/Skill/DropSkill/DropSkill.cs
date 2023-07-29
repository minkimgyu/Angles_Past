using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSkill : MonoBehaviour
{
    [SerializeField]
    string skillName;

    public SkillData ReturnSkill()
    {
        gameObject.SetActive(false);
        return DatabaseManager.Instance.UtilizationDB.ReturnSkillData(skillName);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}