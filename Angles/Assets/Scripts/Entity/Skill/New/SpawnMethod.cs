using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//YellowTriangleEnemy baseFollow;
//baseFollow = new YellowTriangleEnemy(); // 생성자로 새로운 객체 할당 --> 오브젝트 풀링에서 꺼낼 경우 이런 식으로 해당 클레스를 초기화 해보자

abstract public class SpawnMethod<T> : BaseMethod<T>
{
    protected string m_projectileName;
    protected int m_projectileCount;

    public SpawnMethod(string projectileName, int projectileCount, Dictionary<EffectCondition, EffectData> effectDatas) : base(effectDatas)
    {
        m_projectileName = projectileName;
        m_projectileCount = projectileCount;
    }

    [SerializeField]
    protected List<BasicProjectile> spawnedObjects = new List<BasicProjectile>();
    public List<BasicProjectile> SpawnedObjects { get { return spawnedObjects; } }
}

public class SpawnRotationBall : SpawnMethod<GameObject>
{
    float m_distanceFromCaster;

    public SpawnRotationBall(string projectileName, int projectileCount, float distanceFromCaster, Dictionary<EffectCondition, EffectData> effectDatas) : base(projectileName, projectileCount, effectDatas)
    {
        m_distanceFromCaster = distanceFromCaster;
    }

    public override void Execute(SkillSupportData supportData)
    {
        SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, "SpawnBall", 0.05f);

        for (int i = 0; i < m_projectileCount; i++)
        {
            BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(m_projectileName);
            spawnedObjects.Add(projectile);

            projectile.transform.SetParent(supportData.Caster.transform);

        }

        PlayEffect(supportData.Caster.transform, m_effectDatas[EffectCondition.SpawnEffect]);

        for (int j = 0; j < spawnedObjects.Count; j++)
        {
            spawnedObjects[j].transform.position = Vector3.zero;

            float angle = (360.0f * j) / spawnedObjects.Count;

            Vector3 offset = Vector3.up * m_distanceFromCaster;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedOffset = rotation * offset;

            spawnedObjects[j].Init(rotatedOffset);
        }
    }
}

public class SpawnProjectile : SpawnMethod<GameObject>
{
    public SpawnProjectile(string projectileName, int projectileCount, Dictionary<EffectCondition, EffectData> effectDatas) : base(projectileName, projectileCount, effectDatas)
    {
    }

    public override void Execute(SkillSupportData supportData)
    {
        BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(m_projectileName);
        projectile.Init(supportData.Caster.transform);
        spawnedObjects.Add(projectile);
    }
}

public class SpawnAndShootProjectile : SpawnMethod<GameObject>
{
    float m_speed;

    public SpawnAndShootProjectile(string projectileName, int projectileCount, float speed, Dictionary<EffectCondition, EffectData> effectDatas) : base(projectileName, projectileCount, effectDatas)
    {
        m_speed = speed;
    }

    public override void Execute(SkillSupportData supportData)
    {
        BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(m_projectileName);
        projectile.Init(supportData.Caster.transform);
        projectile.Shoot(supportData.Caster.GetComponent<Rigidbody2D>().velocity.normalized, m_speed); // 방향을 보내줌
        spawnedObjects.Add(projectile);
    }
}

public class SpawnBulletInCircleRange : SpawnMethod<GameObject>
{
    float m_speed;
    bool m_isClockwise;
    float m_distanceFromCaster;

    public SpawnBulletInCircleRange(string projectileName, int projectileCount, float speed, bool isClockwise, float distanceFromCaster, Dictionary<EffectCondition, EffectData> effectDatas) 
        : base(projectileName, projectileCount, effectDatas)
    {
        m_speed = speed;
        m_isClockwise = isClockwise;
        m_distanceFromCaster = distanceFromCaster;
    }

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
        float rotation = (360 / m_projectileCount) * supportData.TickCount * RotationDir;

        Vector3 direction = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
        Vector3 tempPos = supportData.Caster.transform.position + direction * m_distanceFromCaster;

        //SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, supportData.Me.Data.SfxName, supportData.Me.Data.Volume);

        BasicBullet projectile = ObjectPooler.SpawnFromPool<BasicBullet>(m_projectileName, tempPos, Quaternion.Euler(0, 0, rotation));
        projectile.Init(tempPos);
        projectile.Fire(direction, m_speed);
        spawnedObjects.Add(projectile);
    }
}