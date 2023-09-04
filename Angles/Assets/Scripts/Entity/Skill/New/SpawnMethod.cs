using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//YellowTriangleEnemy baseFollow;
//baseFollow = new YellowTriangleEnemy(); // �����ڷ� ���ο� ��ü �Ҵ� --> ������Ʈ Ǯ������ ���� ��� �̷� ������ �ش� Ŭ������ �ʱ�ȭ �غ���

abstract public class SpawnMethod<T> : BaseMethod<T>
{
    protected string m_projectileName;

    public SpawnMethod(string projectileName, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(effectDatas, soundDatas)
    {
        m_projectileName = projectileName;
    }
}

public class SpawnRotationBall : SpawnMethod<GameObject>
{
    int m_projectileCount;
    float m_distanceFromCaster;
    protected List<BasicSpawnedObject> spawnedObjects = new List<BasicSpawnedObject>();

    public SpawnRotationBall(string projectileName, int projectileCount, float distanceFromCaster, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(projectileName, effectDatas, soundDatas)
    {
        m_projectileCount = projectileCount;
        m_distanceFromCaster = distanceFromCaster;
    }

    public override void Execute(SkillSupportData supportData)
    {
        SoundManager.Instance.PlaySFX(supportData.Caster.transform.position, "SpawnBall", 0.05f);

        PlayEffect(supportData.Caster.transform, EffectCondition.SpawnEffect);

        for (int i = 0; i < m_projectileCount; i++)
        {
            BasicSpawnedObject projectile = ProjectileFactory.Order(m_projectileName);
            spawnedObjects.Add(projectile);
        }

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            float angle = (360.0f * i) / spawnedObjects.Count;
            spawnedObjects[i].ResetObject(supportData.Caster.transform, angle, m_distanceFromCaster); // trasform���� ���������� --> �̰� �̿��ؼ� ������
        }
    }

    public void RemoveSpawnObject(BasicSpawnedObject projectile)
    {
        spawnedObjects.Remove(projectile);
        if (spawnedObjects.Count == 0)
        {
            // ��ų ����
        }
    }
}

public class SpawnProjectile : SpawnMethod<GameObject> // �̰� �߷��� ��ȯ��ų �� �ֱ�
{
    bool isNeedCaster;

    public SpawnProjectile(string projectileName, bool isNeedCaster, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(projectileName, effectDatas, soundDatas)
    {
        this.isNeedCaster = isNeedCaster;
    }

    public override void Execute(SkillSupportData supportData)
    {
        BasicSpawnedObject projectile = ProjectileFactory.Order(m_projectileName);

        if (isNeedCaster) projectile.ResetObject(supportData.Caster.transform);
        else projectile.ResetObject(supportData.Caster.transform.position);
    }
}

public class SpawnAndShootProjectile : SpawnMethod<GameObject> // bool �� �־ caster�� ������ �ƴϸ� position�� ������ ��������
{
    float m_speed;
    bool isNeedCaster;

    public SpawnAndShootProjectile(string projectileName, float speed, bool isNeedCaster, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(projectileName, effectDatas, soundDatas)
    {
        m_speed = speed;
        this.isNeedCaster = isNeedCaster;
    }

    public override void Execute(SkillSupportData supportData)
    {
        BasicSpawnedObject projectile = ProjectileFactory.Order(m_projectileName);

        if(isNeedCaster) projectile.ResetObject(supportData.Caster.transform);
        else projectile.ResetObject(supportData.Caster.transform.position);


        projectile.TryGetComponent(out IProjectile iprojectile); // �߰��� IProjectile�� ������ �������ش�.
        if (iprojectile == null) return;

        iprojectile.Shoot(supportData.Caster.GetComponent<Rigidbody2D>().velocity.normalized, m_speed); // ������ ������
    }
}

public class SpawnBulletInCircleRange : SpawnMethod<GameObject>
{
    float m_speed;
    bool m_isClockwise;
    float m_distanceFromCaster;

    int m_projectileCount;

    public SpawnBulletInCircleRange(string projectileName, int projectileCount, float speed, bool isClockwise, float distanceFromCaster, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) 
        : base(projectileName, effectDatas, soundDatas)
    {
        m_projectileCount = projectileCount;

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

        BasicSpawnedObject spawnedObject = ProjectileFactory.Order(m_projectileName);
        spawnedObject.ResetObject(tempPos, rotation);

        spawnedObject.TryGetComponent(out IProjectile iprojectile); // �߰��� IProjectile�� ������ �������ش�.
        if (iprojectile == null) return;

        iprojectile.Shoot(direction, m_speed);
    }
}