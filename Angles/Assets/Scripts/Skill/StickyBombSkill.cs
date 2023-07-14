using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StickyBombSkill
{
    //public Color color;

    //Transform explosionTr;
    //Vector3 dirOnContact;

    //CancellationTokenSource _source = new();
    //bool _nowRunning = false;

    //public async UniTaskVoid SkillTask()
    //{
    //    _nowRunning = true;
    //    await UniTask.Delay(TimeSpan.FromSeconds(SkillData.PreDelay), cancellationToken: _source.Token);
    //    DamageToRange();
    //    effect.PlayEffect();

    //    _nowRunning = false;


    //    await UniTask.Delay(TimeSpan.FromSeconds(SkillData.DisableTime), cancellationToken: _source.Token);
    //    DisableObject();
    //}

    //public override void PlaySkill(SkillSupportData data, BasicBattleComponent battleComponent)
    //{
    //    base.PlaySkill(data, battleComponent);
    //    explosionTr = data.contactEntity[0].transform;
    //    dirOnContact = data.contactPos[0] - data.contactEntity[0].transform.position;
    //    SkillTask().Forget();
    //}

    //public void DamageToRange()
    //{
    //    if (explosionTr == null || explosionTr.gameObject.activeSelf == false) return;

    //    transform.position = explosionTr.position;

    //    Vector3 contactPos = transform.position + dirOnContact;
    //    RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, SkillData.RadiusRange, Vector2.up, 0, LayerMask.GetMask("Enemy"));

    //    for (int i = 0; i < hit.Length; i++)
    //    {
    //        if (SkillData.CanHitSkill(hit[i].transform.tag) == false) continue;
    //        DamageToEntity(hit[i].transform.gameObject, SkillData.KnockBackThrust);
    //    }
    //}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = color;
    //    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
    //    Gizmos.DrawWireSphere(Vector3.zero, SkillData.RadiusRange);
    //}

    //public override void PlayEffect()
    //{
    //    throw new NotImplementedException();
    //}

    //private void OnDestroy()
    //{
    //    _source.Cancel();
    //    _source.Dispose();
    //}

    //private void OnEnable()
    //{
    //    if (_source != null)
    //        _source.Dispose();

    //    _source = new();
    //}

    //private void OnDisable()
    //{
    //    _source.Cancel();
    //}
}
