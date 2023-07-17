using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --> 이거를 스크립터블 오브젝트로 만들어서 끼워맞춰주자
[System.Serializable]
[CreateAssetMenu(fileName = "CircleRangeAttack", menuName = "Scriptable Object/DamageMethod/CircleRangeAttack", order = int.MaxValue)]
public class CircleRangeAttack : DamageMethod
{
    public override void Attack(DamageSupportData supportData)
    {
        // 이펙트 생성

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