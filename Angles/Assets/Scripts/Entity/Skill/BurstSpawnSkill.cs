using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSpawnSkill : SpawnSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
        base.Execute(caster);
        spawnMethod.Execute(new SpawnSupportData(caster, this, 1));
    }
}