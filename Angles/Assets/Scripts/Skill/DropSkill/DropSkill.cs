using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSkill : MonoBehaviour
{
    [SerializeField]
    string skillName;

    public SkillCallData ReturnSkill()
    {
        return DatabaseManager.Instance.UtilizationDB.SkillCallDatas.Find(x => x.Name == skillName).CopyData();
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}