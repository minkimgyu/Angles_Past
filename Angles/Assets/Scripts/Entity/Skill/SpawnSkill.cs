using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    [SerializeField]
    List<BasicProjectile> spawnedObjects = new List<BasicProjectile>();
    public List<BasicProjectile> SpawnedObjects { get { return spawnedObjects; } }

    // -- Method �ϳ� �� ���� ��ų�� ���� ���� ���� �ƴϸ� ���� ��ų�� �ѹ� �� ������ �Ǵ�


    // ���Ÿ� ����, �ٰŸ� ���� ��� ���� ��� ���� --> ���� ��ġ�� ��ų ��ġ�� �������� ��
    [SerializeField]
    protected SpawnMethod spawnMethod;
    public SpawnMethod SpawnMethod { get { return spawnMethod; } }

    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
        base.Execute(caster);
        spawnMethod.Execute(new SpawnSupportData(caster, this));
    }

    public void SpawnedObjectDisable(BasicProjectile projectile)
    {
        if(spawnedObjects.Exists(x => x == projectile))
        {
            print("nowExist");
        }

        if (spawnedObjects.Remove(projectile) == true)
        {
            print("ddddddd");
        }

        if (spawnedObjects.Count == 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        spawnedObjects.Clear();
        base.OnDisable();
    }

    private void Update()
    {
        positionMethod.DoUpdate(this);
    }
}
