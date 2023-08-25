using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseEntitySO : BaseSO<Entity> { }

abstract public class BaseFactory<T> : MonoBehaviour
{
    public virtual T Order(string name) { return default; }
    public virtual T Order(GameObject caster, string name) { return default; }
}

public class EntityFactory : BaseFactory<Entity>
{
    [SerializeField]
    StringBaseEntitySODictionary storedEntities;

    public override Entity Order(string name)
    {
        Entity entity = storedEntities[name].Create();
        return entity;
    }
}