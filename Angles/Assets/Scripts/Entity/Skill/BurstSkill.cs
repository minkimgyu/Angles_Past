using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSkill : BasicSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
        base.Execute(caster);
        damageMethod.Attack(new DamageSupportData(caster, gameObject, data));

        gameObject.SetActive(false);
    }
}
