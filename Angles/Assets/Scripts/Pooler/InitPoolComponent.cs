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
        AddToPool("YellowTriangle", "Prefabs/" + "Entity/" + "YellowTriangle", 5);
        AddToPool("YellowRectangle", "Prefabs/" + "Entity/" + "YellowRectangle", 5);
        AddToPool("YellowPentagon", "Prefabs/" + "Entity/" + "YellowPentagon", 5);
        AddToPool("Orb", "Prefabs/Skill/Orb", 3);

        foreach (SkillName skillName in SkillName.GetValues(typeof(SkillName)))
        {
            string name = skillName.ToString();
            if (name == "None") continue;

            //AddToPool(name, "Prefabs/Skill/" + name, 1);

            GameObject prefab = Resources.Load("Prefabs/Skill/" + name) as GameObject;
            //prefab.GetComponent<BasicSkill>().Init();
            ObjectPooler.AddPool(new Pool(name, prefab, 1));
        }
    }

    void AddToPool(string name, string path, int count)
    {
        GameObject prefab = Resources.Load(path) as GameObject;
        ObjectPooler.AddPool(new Pool(name, prefab, count));
    }
}
