using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPoolComponent : MonoBehaviour
{
    private void Awake()
    {
        ObjectPooler.InitAction += Init;
    }

    private void OnDisable()
    {
        ObjectPooler.InitAction -= Init;
    }

    void Init()
    {
        List<EnemyData> enemyData = DatabaseManager.Instance.EntityDB.Enemy;
        List<SkillData> skillData = DatabaseManager.Instance.EntityDB.Skill;
        List<AdditionalPrefabData> AdditionalPrefab = DatabaseManager.Instance.EntityDB.AdditionalPrefab;

        for (int i = 0; i < enemyData.Count; i++)
        {
            AddToPool(enemyData[i].Name, "Prefabs/Entity/" + enemyData[i].Name, enemyData[i].PrefabCount);
        }

        for (int i = 0; i < skillData.Count; i++)
        {
            AddToPool(skillData[i].Name.ToString(), "Prefabs/Skill/" + skillData[i].Name.ToString(), skillData[i].PrefabCount);
        }

        for (int i = 0; i < AdditionalPrefab.Count; i++)
        {
            AddToPool(AdditionalPrefab[i].Name, AdditionalPrefab[i].Path + AdditionalPrefab[i].Name, AdditionalPrefab[i].Count);
        }
    }

    void AddToPool(string name, string path, int count)
    {
        GameObject prefab = Resources.Load(path) as GameObject;
        if (prefab == null) return;

        ObjectPooler.AddPool(new Pool(name, prefab, count));
    }
}
