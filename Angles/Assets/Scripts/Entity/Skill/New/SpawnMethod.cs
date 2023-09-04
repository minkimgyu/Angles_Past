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

        PlayEffect(supportData.Caster.transform, EffectCondition.SpawnEffect);

        for (int i = 0; i < m_projectileCount; i++)
        {
            BasicSpawnedObject projectile = ProjectileFactory.Order(m_projectileName);
            spawnedObjects.Add(projectile);
        }

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            float angle = (360.0f * i) / spawnedObjects.Count;
            spawnedObjects[i].ResetObject(supportData.Caster.transform, angle, m_distanceFromCaster); // trasform으로 수정해주자 --> 이걸 이용해서 돌리기
        }
    }

    public void RemoveSpawnObject(BasicSpawnedObject projectile)
    {
        spawnedObjects.Remove(projectile);
        if (spawnedObjects.Count == 0)
        {
            // 스킬 종료
        }
    }
}

public class SpawnProjectile : SpawnMethod<GameObject> // 이건 중력장 소환시킬 때 넣기
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

public class SpawnAndShootProjectile : SpawnMethod<GameObject> // bool 값 넣어서 caster를 넣을지 아니면 position만 넣을지 선택하자
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