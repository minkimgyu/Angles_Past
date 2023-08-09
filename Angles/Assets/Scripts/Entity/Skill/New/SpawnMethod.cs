using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SpawnMethod<T> : BaseMethod<T>
{
    [SerializeField]
    protected List<BasicProjectile> spawnedObjects = new List<BasicProjectile>();
    public List<BasicProjectile> SpawnedObjects { get { return spawnedObjects; } }

    protected SpawnSupportData spawnSkillData;

    public override void Init(SkillData data) // 여기서 불러옴
    {
        //damageStat = DatabaseManager.Instance.db
    }
}

public class SpawnRotationBall : SpawnMethod<GameObject>
{
    public override void Execute(SkillSupportData supportData)
    {
        SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, "SpawnBall", 0.05f);

        for (int i = 0; i < supportData.Data.SpawnCount; i++)
        {
            BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(spawnSkillData.projectileName);
            spawnedObjects.Add(projectile);

            projectile.transform.SetParent(supportData.Caster.transform);

        }

        PlayEffect(supportData.Caster.transform, spawnSkillData.effectDatas[EffectName.SpawnEffect]);

        for (int j = 0; j < spawnedObjects.Count; j++)
        {
            spawnedObjects[j].transform.position = Vector3.zero;

            float angle = (360.0f * j) / spawnedObjects.Count;

            Vector3 offset = Vector3.up * spawnSkillData.distanceFromCaster;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedOffset = rotation * offset;

            spawnedObjects[j].Init(rotatedOffset);
        }
    }
}

public class SpawnProjectile : SpawnMethod<GameObject>
{
    public override void Execute(SkillSupportData supportData)
    {
        BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(spawnSkillData.projectileName);
        projectile.Init(supportData.Caster.transform);
        spawnedObjects.Add(projectile);
    }
}

public class SpawnAndShootProjectile : SpawnMethod<GameObject>
{
    public override void Execute(SkillSupportData supportData)
    {
        BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(spawnSkillData.projectileName);
        projectile.Init(supportData.Caster.transform);
        projectile.Shoot(supportData.Caster.GetComponent<Rigidbody2D>().velocity.normalized, spawnSkillData.speed); // 방향을 보내줌
        spawnedObjects.Add(projectile);
    }
}

public class SpawnBulletInCircleRange : SpawnMethod<GameObject>
{
    public SpawnBulletInCircleRange(bool isClockwise)
    {
        m_isClockwise = isClockwise;
    }

    bool m_isClockwise; // 시계 방향으로 쏠건지 유무
    int RotationDir
    {
        get
        {
            if (m_isClockwise) return 1;
            else return - 1;
        }
    }

    public override void Execute(SkillSupportData supportData)
    {
        float rotation = (360 / spawnSkillData.projectileCount) * supportData.TickCount * RotationDir;

        Vector3 direction = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
        Vector3 tempPos = supportData.Caster.transform.position + direction * spawnSkillData.distanceFromCaster;

        //SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, supportData.Me.Data.SfxName, supportData.Me.Data.Volume);

        BasicBullet projectile = ObjectPooler.SpawnFromPool<BasicBullet>(spawnSkillData.projectileName, tempPos, Quaternion.Euler(0, 0, rotation));
        projectile.Init(tempPos);
        projectile.Fire(direction, spawnSkillData.speed);
        spawnedObjects.Add(projectile);
    }
}