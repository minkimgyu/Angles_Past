using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpeedTimerBuffSO", menuName = "Scriptable Object/BuffSO/SpeedTimerBuffSO")]
public class SpeedTimerBuffSO : BaseBuffSO
{
    [SerializeField]
    string buffName;

    [SerializeField]
    string effectName;

    [SerializeField]
    int maxCount;

    [SerializeField]
    float duration;

    [SerializeField]
    float maxTickCount;

    [SerializeField]
    float speedVariation;

    public override BaseBuff Create()
    {
        return new SpeedTimerBuff(buffName, maxCount, effectName, duration, maxTickCount, speedVariation);
    }
}
