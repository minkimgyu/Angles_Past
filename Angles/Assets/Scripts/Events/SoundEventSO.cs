using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "SoundEventSO", menuName = "Events/SoundEventSO")]
public class SoundEventSO : ScriptableObject
{
    public UnityAction OnActionRequested;

    public void RaiseEvent()
    {
        if (OnActionRequested == null) return;

        OnActionRequested.Invoke();
    }
}
