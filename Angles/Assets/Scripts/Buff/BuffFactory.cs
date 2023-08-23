using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSO<T> : ScriptableObject
{
    public abstract T Create();
}
  

abstract public class BaseBuffSO : BaseSO<BaseBuff> { }


public class BuffFactory : MonoBehaviour // 하나만 만들자 --> 스킬 팩토리도 마찬가지
{
    [SerializeField]
    StringBaseBuffSODictionary storedBuffSO;

    Dictionary<string, BaseBuff> storedBuffs; // SO 만들어서 데이터 저장소에서 불러옴

    public BaseBuff OrderBuff(string name)
    {
        return storedBuffs[name]; // 이런 식으로 반환
    }
}
