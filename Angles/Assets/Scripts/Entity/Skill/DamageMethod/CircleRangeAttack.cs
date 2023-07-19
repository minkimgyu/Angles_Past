using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --> �̰Ÿ� ��ũ���ͺ� ������Ʈ�� ���� ������������
[System.Serializable]
[CreateAssetMenu(fileName = "CircleRangeAttack", menuName = "Scriptable Object/DamageMethod/CircleRangeAttack", order = int.MaxValue)]
public class CircleRangeAttack : DamageMethod
{
    public override void Execute(DamageSupportData supportData)
    {
        // ����Ʈ ����

        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Me.transform.position, supportData.Me.Data.RadiusRange, Vector2.up, 0);
        for (int i = 0; i < hit.Length; i++)
        {
            DamageToEntity(supportData.Me.gameObject, hit[i].transform, supportData.Me.Data);
        }


        BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
        if (effectPlayer == null) return;

        supportData.Me.EffectPlayer = effectPlayer; // Effect ���� �ʱ�ȭ

        effectPlayer.Init(supportData.Me.transform, supportData.Me.Data.DisableTime);
        effectPlayer.PlayEffect();
    }
}