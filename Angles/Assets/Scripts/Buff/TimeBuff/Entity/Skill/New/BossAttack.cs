using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//[CreateAssetMenu(fileName = "BossAttack", menuName = "Scriptable Object/DamageMethod/BossAttack", order = int.MaxValue)]
//public class BossAttack : DamageMethod
//{
//    [SerializeField]
//    protected EffectMethod effectMethod2; // 효과들 모음 --> 이팩트는 이름으로 오브젝트 풀링에서 불러옴

//    [SerializeField]
//    protected EffectMethod effectMethod3; // 효과들 모음 --> 이팩트는 이름으로 오브젝트 풀링에서 불러옴

//    [SerializeField]
//    bool exceptMe;

//    [SerializeField]
//    float[] range = { 1.5f, 2f, 3f };

//    public override void Execute(DamageSupportData supportData)
//    {
//        if(supportData.m_TickCount == 1)
//        {
//            Attack(supportData);
//            Effect(supportData, effectMethod);
//        }
//        else if(supportData.m_TickCount == 2)
//        {
//            Attack(supportData);
//            Effect(supportData, effectMethod2);
//        }
//        else if (supportData.m_TickCount == 3)
//        {
//            Attack(supportData);
//            Effect(supportData, effectMethod3);
//        }

//        SoundManager.Instance.PlaySFX(supportData.Me.transform.position, supportData.Me.Data.SfxName, supportData.Me.Data.Volume);
//    }

//    void Attack(DamageSupportData supportData)
//    {
//        RaycastHit2D[] hit = Physics2D.CircleCastAll(supportData.Me.transform.position, range[supportData.m_TickCount - 1], Vector2.up, 0);
//        for (int i = 0; i < hit.Length; i++)
//        {
//            if (exceptMe == true)
//            {
//                if (hit[i].transform.gameObject == supportData.Caster) continue;
//            }

//            DamageToEntity(supportData.Me.gameObject, hit[i].transform, supportData.Me.Data);
//        }
//    }

//    void Effect(DamageSupportData supportData, EffectMethod effectMethod)
//    {
//        BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();

//        if (effectPlayer == null) return;

//        effectPlayer.AddState(supportData.Me.transform, supportData.Me.Data.DisableTime);
//        effectPlayer.PlayEffect();
//    }
//}
