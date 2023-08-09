using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "SkillCueEventSO", menuName = "Events/SkillCueEventSO")]
public class SkillCueEventSO : ScriptableObject
{
    public UnityAction<Transform, SkillData> OnSkillCueRequested;

    public void RaiseEvent(Transform caster, SkillData data)
    {
        if (OnSkillCueRequested == null) return;

        OnSkillCueRequested.Invoke(caster, data);
    }
}
