using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = int.MaxValue)]

public class ScriptableSkillData : ScriptableObject
{
    public SkillData skillData;
    public GameObject skillPrefab; // 이걸 사용해서 풀에 오브젝트 생성, 꺼내기 활용

    public void Exetute()
    {
        // pool에서 스킬 꺼내서 사용하기
        // 여기서 Init 시작 --> this 넘겨주기
    }
}
