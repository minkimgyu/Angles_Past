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
        AddToPool("Bullet", "Prefabs/" + "Entity/" + "Bullet", 5);
        AddToPool("Triangle", "Prefabs/" + "Entity/" + "Triangle", 5);
        AddToPool("Rectangle", "Prefabs/" + "Entity/" + "Rectangle", 5);
        AddToPool("Orb", "Prefabs/Skill/Orb", 3);

        foreach (SkillName skillName in SkillName.GetValues(typeof(SkillName)))
        {
            string name = skillName.ToString();
            if (name == "None") continue;

            AddToPool(name, "Prefabs/Skill/" + name, 1);
        }
    }

    void AddToPool(string name, string path, int count)
    {
        GameObject prefab = Resources.Load(path) as GameObject;
        ObjectPooler.AddPool(new Pool(name, prefab, count));
    }
}
