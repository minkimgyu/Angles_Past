using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --> 이거를 스크립터블 오브젝트로 만들어서 끼워맞춰주자
[System.Serializable]
[CreateAssetMenu(fileName = "CircleRangeAttack", menuName = "Scriptable Object/DamageMethod/CircleRangeAttack", order = int.MaxValue)]
public class CircleRangeAttack : DamageMethod
{
    [SerializeField]
    EffectMethod hitEffect;

    [SerializeField]
    bool exceptMe;

    public override void Execute(DamageSupportData supportData)
    {
        // 이펙트 생성

        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Me.transform.position, supportData.Me.Data.RadiusRange, Vector2.up, 0);
        for (int i = 0; i < hit.Length; i++)
        {
            if(exceptMe == true)
            {
                if (hit[i].transform.gameObject == supportData.Caster) continue;
            }


            if(!DamageToEntity(supportData.Me.gameObject, hit[i].transform, supportData.Me.Data)) continue;

            ShowNomalHitEffect(supportData, hit[i].transform);
        }


        SoundManager.Instance.PlaySFX(supportData.Me.transform.position, supportData.Me.Data.SfxName, supportData.Me.Data.Volume);

        BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();

        if (effectPlayer == null) return;

        supportData.Me.EffectPlayer = effectPlayer; // Effect 변수 초기화

        effectPlayer.Init(supportData.Me.transform, supportData.Me.Data.DisableTime);
        effectPlayer.PlayEffect();
    }
    
    void ShowNomalHitEffect(DamageSupportData supportData, Transform hitTr)
    {
        BasicEffectPlayer effectPlayer = hitEffect.ReturnEffectFromPool();

        if (effectPlayer == null) return;

        supportData.Me.EffectPlayer = effectPlayer; // Effect 변수 초기화

        effectPlayer.Init(hitTr.position, supportData.Me.Data.DisableTime);
        effectPlayer.PlayEffect();
    }
}