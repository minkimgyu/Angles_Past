using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = int.MaxValue)]

public class ScriptableSkillData : ScriptableObject
{
    public SkillData skillData;
    public GameObject skillPrefab; // �̰� ����ؼ� Ǯ�� ������Ʈ ����, ������ Ȱ��

    public void Exetute()
    {
        // pool���� ��ų ������ ����ϱ�
        // ���⼭ Init ���� --> this �Ѱ��ֱ�
    }
}
