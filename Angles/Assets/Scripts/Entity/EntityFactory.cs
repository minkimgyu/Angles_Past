using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseEntitySO : BaseSO<Entity> { }

public class EntityFactory : MonoBehaviour
{
    [SerializeField]
    StringBaseEntitySODictionary storedEntityies;

    public Entity OrderEntity(string name)
    {
        Entity entity = storedEntityies[name].Create();
        return entity;
    }
}