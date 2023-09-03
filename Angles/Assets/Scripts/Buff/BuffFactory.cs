using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseBuffSO : BaseSO<BaseBuff> { }

public class BuffFactory : MonoBehaviour // 하나만 만들자 --> 스킬 팩토리도 마찬가지
{
    [SerializeField]
    StringBaseBuffSODictionary storedBuffSO;

    static BuffFactory inst;
    private void Awake() => inst = this;

    public static BaseBuff Order(string name)
    {
        BaseBuff entity = inst.storedBuffSO[name].Create();
        return entity;
    }
}
