using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSO<T> : ScriptableObject
{
    public abstract T Create();
}
  

abstract public class BaseBuffSO : BaseSO<BaseBuff> { }


public class BuffFactory : MonoBehaviour // �ϳ��� ������ --> ��ų ���丮�� ��������
{
    [SerializeField]
    StringBaseBuffSODictionary storedBuffSO;

    Dictionary<string, BaseBuff> storedBuffs; // SO ���� ������ ����ҿ��� �ҷ���

    public BaseBuff OrderBuff(string name)
    {
        return storedBuffs[name]; // �̷� ������ ��ȯ
    }
}
