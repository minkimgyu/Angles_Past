using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Enemy<T> : Avatar<T>
{
    protected BuffInt score;
    public BuffInt Score { get { return score; }}

    protected BuffFloat spawnPercentage;
    public BuffFloat SpawnPercentage { get { return spawnPercentage; } }

    BuffInt goldCount;

    [SerializeField]
    Sprite sprite;

    bool isHpBelowHalf = false;

    protected virtual void Start() => AddState();

    protected abstract void AddState();

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
    BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames, BuffInt score, BuffInt goldCount, BuffFloat spawnPercentage)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames);

        this.score = score.CopyData();
        this.goldCount = goldCount.CopyData();
        this.spawnPercentage = spawnPercentage.CopyData();
        isHpBelowHalf = false;
    }

    public override void Die()
    {
        DashComponent.QuickEndTask();
        DropGold();
        DropSkill();
        base.Die();
    }

    private void DropSkill()
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        if (random <= spawnPercentage.IntervalValue)
        {
            List<string> skillNames = SaveManager.Instance.ReturnSkills();
            if (skillNames.Count == 0) return;

            string pickedSkillName = skillNames[UnityEngine.Random.Range(0, skillNames.Count)];

            DropSkill skill = ObjectPooler.SpawnFromPool<DropSkill>(pickedSkillName); // 여기에서 데이터베이스 접근 후, 스킬 이름 배열로 들고오기
            skill.transform.position = transform.position;
        }
    }

    void DropGold()
    {
        float x = UnityEngine.Random.Range(-1.0f, 1.0f) + transform.position.x;
        float y = UnityEngine.Random.Range(-1.0f, 1.0f) + transform.position.y;

        for (int i = 0; i < goldCount.IntervalValue; i++)
        {
            Gold gold = ObjectPooler.SpawnFromPool<Gold>("Gold");
            gold.transform.position = new Vector2(x, y);
        }
    }

    public override void WhenUnderAttack(float damage, Vector2 dir, float thrust)
    {
        base.WhenUnderAttack(damage, dir, thrust);
        if (maxHp / 2 >= Hp.IntervalValue && isHpBelowHalf == false && sprite != null)
        {
            SpriteRenderer.sprite = sprite;
            isHpBelowHalf = true;
        }
    }
}