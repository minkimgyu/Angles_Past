using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Enemy<T> : Avatar<T>
{
    //BaseEnemyData baseEnemyData;

    //string[] dropSkills = { "GhostItem", "BarrierItem", "BladeItem", "KnockbackItem", "ShockwaveItem", "SpawnBallItem", "SpawnGravityBallItem", "StickyBombItem" };

    //void SpawnRandomItem()
    //{
    //    float percentage = UnityEngine.Random.Range(0.0f, 1.0f);
    //    if (percentage > baseEnemyData.SpawnPercentage) return;

    //    ObjectPooler.SpawnFromPool<DropSkill>(dropSkills[UnityEngine.Random.Range(0, dropSkills.Length)], transform.position);
    //}

    // 따로 스킬 아이템 스포너를 만들자

    protected BuffInt score;
    public BuffInt Score { get { return score; }}

    protected virtual void Start() => AddState();

    protected abstract void AddState();

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
    BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames, BuffInt score)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames);

        this.score = score;
    }
}