using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnCueEventSO", menuName = "Events/SpawnCueEventSO")]
public class SpawnCueEventSO : ScriptableObject
{
    public UnityAction<Vector3, int, string> OnActionRequested;

    public void RaiseEvent(Vector3 pos, int count, string name)
    {
        if (OnActionRequested == null) return;

        OnActionRequested.Invoke(pos, count, name);
    }
}
