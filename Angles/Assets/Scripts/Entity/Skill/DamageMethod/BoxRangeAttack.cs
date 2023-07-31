using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BoxRangeAttack", menuName = "Scriptable Object/DamageMethod/BoxRangeAttack", order = int.MaxValue)]
public class BoxRangeAttack : DamageMethod
{
    public override void Execute(DamageSupportData supportData) 
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(supportData.Me.transform.position, supportData.Me.Data.BoxRange, 
            supportData.Caster.transform.rotation.z, Vector2.right, supportData.Me.Data.OffsetRange.magnitude);

        for (int i = 0; i < hit.Length; i++)
        {
            DamageToEntity(supportData.Me.gameObject, hit[i].transform, supportData.Me.Data);
        }


        SoundManager.Instance.PlaySFX(supportData.Me.transform.position, supportData.Me.Data.SfxName, supportData.Me.Data.Volume);

        BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
        if (effectPlayer == null) return;

        supportData.Me.EffectPlayer = effectPlayer; // Effect 변수 초기화

        float tempRotation = supportData.Caster.transform.eulerAngles.z; // 로테이션 돌려서 이펙트 보여주기
        effectPlayer.RotationEffect(tempRotation);

        effectPlayer.Init(supportData.Me.transform, supportData.Me.Data.DisableTime);
        effectPlayer.PlayEffect();
    }
}
