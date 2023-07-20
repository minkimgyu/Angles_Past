using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    [SerializeField]
    List<BasicProjectile> spawnedObjects = new List<BasicProjectile>();

    public List<BasicProjectile> SpawnedObjects { get { return spawnedObjects; } }

    // ���Ÿ� ����, �ٰŸ� ���� ��� ���� ��� ���� --> ���� ��ġ�� ��ų ��ġ�� �������� ��
    [SerializeField]
    protected SpawnMethod spawnMethod;
    public SpawnMethod SpawnMethod { get { return spawnMethod; } }
    // -- Method �ϳ� �� ���� ��ų�� ���� ���� ���� �ƴϸ� ���� ��ų�� �ѹ� �� ������ �Ǵ�

    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
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