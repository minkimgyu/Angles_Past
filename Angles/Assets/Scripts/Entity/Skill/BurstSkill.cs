using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSkill : AttackSkill // --> ���������� �����ؼ� ������Ʈ Ǯ���� �߰�
{
    public override void Execute(GameObject caster) // ���⿡�� Unitask ȣ���ؼ� ƽ �� ������ ���� �Լ� �߰�
    {
        // --> ��ġ ���� ���� �߰�
        base.Execute(caster);
        damageMethod.Execute(new DamageSupportData(caster, this));



        gameObject.SetActive(false);
    }
}
