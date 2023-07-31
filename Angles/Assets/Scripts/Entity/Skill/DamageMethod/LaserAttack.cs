using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LaserAttack", menuName = "Scriptable Object/DamageMethod/LaserAttack", order = int.MaxValue)]

public class LaserAttack : DamageMethod
{
    [SerializeField]
    protected EffectMethod hitEffectMethod; // 효과들 모음 --> 이팩트는 이름으로 오브젝트 풀링에서 불러옴

    [SerializeField]
    float hitEffectDisableTime  = 1.5f;

    [SerializeField]   
    float maxDistance = 20;

    [SerializeField]
    List<string> blockedTag;

    public override void Execute(DamageSupportData supportData)
    {
        List<Vector3> hitPos = new List<Vector3>();
        List<Vector3> hitEffectPos = new List<Vector3>();

        int ignoreLayer = supportData.Caster.layer;

        RaycastHit2D[] hits = Physics2D.RaycastAll(supportData.Me.transform.position, supportData.Me.Data.Directions[supportData.m_TickCount - 1], maxDistance);
        Debug.DrawRay(supportData.Me.transform.position, supportData.Me.Data.Directions[supportData.m_TickCount - 1].normalized, Color.green, maxDistance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == null || hits[i].transform == supportData.Caster.transform) continue;

            bool isBlocked = false;

            for (int j = 0; j < blockedTag.Count; j++)
            {
                if (hits[i].transform.tag == blockedTag[j])
                {
                    hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
                    hitEffectPos.Add(hits[i].point);
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked) break;

            if (DamageToEntity(supportData.Me.gameObject, hits[i].transform, supportData.Me.Data) == false) continue;

            hitPos.Add(hits[i].point - (Vector2)supportData.Caster.transform.position);
            hitEffectPos.Add(hits[i].point);

            break;
        }

        if (hitPos.Count == 0)
        {
            hitPos.Add(supportData.Me.Data.Directions[supportData.m_TickCount - 1] * maxDistance / 2);
        }

        for (int i = 0; i < hitEffectPos.Count; i++)
        {
            BasicEffectPlayer hitEffectPlayer = hitEffectMethod.ReturnEffectFromPool();

            hitEffectPlayer.Init(hitEffectPos[i], hitEffectDisableTime);
            hitEffectPlayer.PlayEffect();
        }

        SoundManager.Instance.PlaySFX(supportData.Me.transform.position, supportData.Me.Data.SfxName, supportData.Me.Data.Volume);

        BasicEffectPlayer effectPlayer = effectMethod.ReturnEffectFromPool();
        if (effectPlayer == null) return;

        supportData.Me.EffectPlayer = effectPlayer; // Effect 변수 초기화

        effectPlayer.Init(supportData.Me.transform, supportData.Me.Data.DisableTime, hitPos);
        effectPlayer.PlayEffect();
    }
}
