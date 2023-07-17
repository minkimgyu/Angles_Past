using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --> �̰Ÿ� ��ũ���ͺ� ������Ʈ�� ���� ������������
[System.Serializable]
[CreateAssetMenu(fileName = "CircleRangeAttack", menuName = "Scriptable Object/DamageMethod/CircleRangeAttack", order = int.MaxValue)]
public class CircleRangeAttack : DamageMethod
{
    public override void Attack(DamageSupportData supportData)
    {
        // ����Ʈ ����

        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Me.transform.position, supportData.Data.RadiusRange, Vector2.up, 0);
        for (int i = 0; i < hit.Length; i++)
        {
            if(DamageToEntity(supportData.Me, hit[i].transform, supportData.Data) == true)
            {
                BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
                effectPlayer.Init(supportData.Me.transform);
                effectPlayer.PlayEffect();
            }
        }
    }
}