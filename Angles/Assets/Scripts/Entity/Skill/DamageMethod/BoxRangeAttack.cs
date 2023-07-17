using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BoxRangeAttack", menuName = "Scriptable Object/DamageMethod/BoxRangeAttack", order = int.MaxValue)]
public class BoxRangeAttack : DamageMethod
{
    public override void Attack(DamageSupportData supportData) 
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(supportData.Me.transform.position, supportData.Data.BoxRange, 
            supportData.Caster.transform.rotation.z, Vector2.right, supportData.Data.OffsetRange.magnitude);

        for (int i = 0; i < hit.Length; i++)
        {
            if (DamageToEntity(supportData.Me, hit[i].transform, supportData.Data) == true)
            {
                BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();

                float tempRotation = supportData.Caster.transform.eulerAngles.z; // 로테이션 돌려서 이펙트 보여주기
                effectPlayer.RotationEffect(tempRotation);

                effectPlayer.Init(supportData.Me.transform);
                effectPlayer.PlayEffect();
            }
        }
    }
}
