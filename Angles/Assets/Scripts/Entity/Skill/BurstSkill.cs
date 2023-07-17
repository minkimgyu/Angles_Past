using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSkill : BasicSkill // --> 프리팹으로 생성해서 오브젝트 풀링에 추가
{
    public override void Execute(GameObject caster) // 여기에서 Unitask 호출해서 틱 당 데미지 적용 함수 추가
    {
        // --> 위치 지정 로직 추가
        base.Execute(caster);
        damageMethod.Attack(new DamageSupportData(caster, gameObject, data));

        gameObject.SetActive(false);
    }
}
