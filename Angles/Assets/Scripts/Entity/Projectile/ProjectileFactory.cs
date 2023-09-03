using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BasicProjectileSO : BaseSO<BasicSpawnedObject> { }

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField]
    StringBasicProjectileSODictionary storedProjectiles;

    static ProjectileFactory inst;
    private void Awake() => inst = this;

    public static BasicSpawnedObject Order(string name)
    {
        BasicSpawnedObject entity = inst.storedProjectiles[name].Create();
        return entity;
    }
}
