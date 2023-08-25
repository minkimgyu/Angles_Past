using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSO<T> : ScriptableObject
{
    public abstract T Create();
}
  

abstract public class BaseBuffSO : BaseSO<BaseBuff> { }


public class BuffFactory : BaseFactory<BaseBuff> // 하나만 만들자 --> 스킬 팩토리도 마찬가지
{
    [SerializeField]
    StringBaseBuffSODictionary storedBuffSO;

    public override BaseBuff Order(string name)
    {
        BaseBuff entity = storedBuffSO[name].Create();
        return entity;
    }
}
