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
        //List<AdditionalPrefabData> additionalPrefab = DatabaseManager.Instance.EntityDB.AdditionalPrefab;

        for (int i = 0; i < enemyData.Count; i++)
        {
            AddToPool(enemyData[i].Name, "Prefabs/Entity/" + enemyData[i].Name, enemyData[i].PrefabCount);
        }

        for (int i = 0; i < skillData.Count; i++)
        {
            AddToPool(skillData[i].Name.ToString(), "Prefabs/Skill/" + skillData[i].Name.ToString(), skillData[i].PrefabCount);
        }

        //for (int i = 0; i < additionalPrefab.Count; i++)
        //{
        //    AddToPool(additionalPrefab[i].Name, additionalPrefab[i].Path + additionalPrefab[i].Name, additionalPrefab[i].Count);
        //}
    }

    void AddToPool(string name, string path, int count)
    {
        path = path.TrimEnd((char)13);

        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null) return;

        ObjectPooler.AddPool(new Pool(name, prefab, count));
    }
}
