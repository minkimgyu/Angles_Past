using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BasicSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    [SerializeField]
    List<BasicProjectile> spawnedObjects = new List<BasicProjectile>();

    public List<BasicProjectile> SpawnedObjects { get { return spawnedObjects; } }

    // 원거리 공격, 근거리 공격 등등 공격 방식 설정 --> 시전 위치는 스킬 위치를 기준으로 함
    [SerializeField]
    protected SpawnMethod spawnMethod;
    public SpawnMethod SpawnMethod { get { return spawnMethod; } }
    // -- Method 하나 더 만들어서 스킬이 새로 사용될 건지 아니면 기존 스킬을 한번 더 쓸건지 판단

    public override void Execute(GameObject caster) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        // --> 위치 지정 로직 추가
        base.Execute(caster);
        spawnMethod.Execute(new SpawnSupportData(caster, this));
    }

    protected override void OnDisable()
    {
        spawnedObjects.Clear();
        base.OnDisable();
    }

    private void Update()
    {
        positionMethod.DoUpdate(this);
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            spawnedObjects[i].DoUpdate();
            if(spawnedObjects[i].IsFinished)
            {
                spawnedObjects[i].OnEnd();
                spawnedObjects.Remove(spawnedObjects[i]);
                if (spawnedObjects.Count == 0)
                {
                    IsFinished = true;
                }
            }
        }
    }
}