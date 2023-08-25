using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSO<T> : ScriptableObject
{
    public abstract T Create();
}
  

abstract public class BaseBuffSO : BaseSO<BaseBuff> { }


public class BuffFactory : BaseFactory<BaseBuff> // �ϳ��� ������ --> ��ų ���丮�� ��������
{
    [SerializeField]
    StringBaseBuffSODictionary storedBuffSO;

    public override BaseBuff Order(string name)
    {
        BaseBuff entity = storedBuffSO[name].Create();
        return entity;
    }
}
