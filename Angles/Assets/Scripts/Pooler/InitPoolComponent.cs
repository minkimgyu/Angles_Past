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
        AddToPool("Enemy", "Prefabs/" + "Entity/" + "Enemy", 2);

        foreach (SkillName skillName in SkillName.GetValues(typeof(SkillName)))
        {
            string name = skillName.ToString();
            string effect = name + "Effect";

            if (name == "None") continue;

            AddToPool(name, "Prefabs/" + "Skill/" + name, 2);
            AddToPool(effect, "Prefabs/" + "Effect/" + effect, 2);
        }
    }

    void AddToPool(string name, string path, int count)
    {
        GameObject prefab = Resources.Load(path) as GameObject;
        ObjectPooler.AddPool(new Pool(name, prefab, count));
    }
}
