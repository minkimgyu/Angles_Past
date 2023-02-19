using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class GravitationalFieldSkill : BasicSkill
{
    [SerializeField]
    List<ForceComponent> forceComponents = new List<ForceComponent>();

    float damageTime = 5f;
    float damagePerTime = 0.1f; // 10

    protected override void OnEnable()
    {
        WhenEnable();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        { 
            forceComponents.Add(col.GetComponent<ForceComponent>());
            print(col);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) forceComponents.Remove(col.GetComponent<ForceComponent>());
    }

    void SetGravity()
    {
        for (int i = 0; i < forceComponents.Count; i++)
        {
            Vector3 dir = -(forceComponents[i].transform.position - transform.position).normalized * 15;
            forceComponents[i].AddForceUsingVec(dir, ForceMode2D.Force);
        }
    }

    public async UniTaskVoid SkillTask()
    {
        effect.PlayEffect();
        NowRunning = true;

        int damageTic = 0;
        int maxDamageTic = (int)(damageTime / damagePerTime);

        while (damageTic < maxDamageTic)
        {
            SetGravity();
            await UniTask.Delay(TimeSpan.FromSeconds(damagePerTime), cancellationToken: source.Token);
            damageTic += 1;
        }

        NowRunning = false;
        effect.StopEffect();
    }

    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        transform.position = skillSupportData.player.transform.position;
        base.PlaySkill(skillSupportData);
        SkillTask().Forget();
    }
}
