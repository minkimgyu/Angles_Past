using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//YellowTriangleEnemy baseFollow;
//baseFollow = new YellowTriangleEnemy(); // 생성자로 새로운 객체 할당 --> 오브젝트 풀링에서 꺼낼 경우 이런 식으로 해당 클레스를 초기화 해보자

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

        for (int i = 0; i < m_projectileCount; i++)
        {
            BasicSpawnedObject projectile = ObjectPooler.SpawnFromPool<BasicSpawnedObject>(m_projectileName);
            //spawnedObjects.Add(projectile);

            projectile.transform.SetParent(supportData.Caster.transform);

        }

        PlayEffect(supportData.Caster.transform, EffectCondition.SpawnEffect);

        for (int j = 0; j < spawnedObjects.Count; j++)
        {
            spawnedObjects[j].transform.position = Vector3.zero;

            float angle = (360.0f * j) / spawnedObjects.Count;

            Vector3 offset = Vector3.up * m_distanceFromCaster;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedOffset = rotation * offset;

            spawnedObjects[j].ResetObject(supportData.Caster.transform, rotatedOffset); // trasform으로 수정해주자 --> 이걸 이용해서 돌리기
        }
    }
}

public class SpawnProjectile : SpawnMethod<GameObject> // 이건 중력장 소환시킬 때 넣기
{
    public SpawnProjectile(string projectileName, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(projectileName, effectDatas, soundDatas)
    {
    }

    public override void Execute(SkillSupportData supportData)
    {
        BasicSpawnedObject projectile = ObjectPooler.SpawnFromPool<BasicSpawnedObject>(m_projectileName);
        projectile.ResetObject(supportData.Caster.transform.position);
    }
}

public class SpawnAndShootProjectile : SpawnMethod<GameObject> // bool 값 넣어서 caster를 넣을지 아니면 position만 넣을지 선택하자
{
    float m_speed;

    public SpawnAndShootProjectile(string projectileName, float speed, Dictionary<EffectCondition, EffectData> effectDatas, Dictionary<EffectCondition, SoundData> soundDatas) : base(projectileName, effectDatas, soundDatas)
    {
        m_speed = speed;
    }

    public override void Execute(SkillSupportData supportData)
    {
        BasicSpawnedObject projectile = ObjectPooler.SpawnFromPool<BasicSpawnedObject>(m_projectileName);
        projectile.ResetObject(supportData.Caster.transform.position);

        projectile.TryGetComponent(out IProjectile iprojectile); // 추가로 IProjectile의 구현을 실행해준다.
        if (iprojectile == null) return;

        iprojectile.Shoot(supportData.Caster.GetComponent<Rigidbody2D>().velocity.normalized, m_speed); // 방향을 보내줌
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

        spawnedObject.TryGetComponent(out IProjectile iprojectile); // 추가로 IProjectile의 구현을 실행해준다.
        if (iprojectile == null) return;

        iprojectile.Shoot(direction, m_speed);
    }
}