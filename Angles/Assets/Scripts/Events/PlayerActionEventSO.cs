using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerActionEventSO", menuName = "Events/PlayerActionEventSO")]
public class PlayerActionEventSO : ScriptableObject
{
    public UnityAction<float> OnActionRequested;

    public void RaiseEvent(float ratio)
    {
        if (OnActionRequested == null) return;

        OnActionRequested.Invoke(ratio);
    }
}
