using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSO<T> : ScriptableObject
{
    public abstract T Create();
}

abstract public class BaseEntitySO : BaseSO<Entity> { }

public class EntityFactory : MonoBehaviour
{
    [SerializeField]
    StringBaseEntitySODictionary storedEntities;

    static EntityFactory inst;
    private void Awake() => inst = this;

    public static Entity Order(string name)
    {
        Entity entity = inst.storedEntities[name].Create();
        return entity;
    }
}