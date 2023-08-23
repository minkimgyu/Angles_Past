using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpeedBuffSO", menuName = "Scriptable Object/BuffSO/SpeedBuffSO")]
public class SpeedBuffSO : BaseBuffSO
{
    [SerializeField]
    string buffName;

    [SerializeField]
    int maxCount;

    [SerializeField]
    float speedVariation;

    public override BaseBuff Create()
    {
        return new SpeedBuff(buffName, maxCount, speedVariation);
    }
}
