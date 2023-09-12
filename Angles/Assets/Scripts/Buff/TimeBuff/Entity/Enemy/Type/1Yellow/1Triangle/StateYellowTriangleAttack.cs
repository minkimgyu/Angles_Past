using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateYellowTriangleAttack : StateFollowEnemyAttack
{
    List<BaseBuff> storedBuff = new List<BaseBuff>();

    BasicEffectPlayer effectPlayer;
    string skillEffectName;

    public StateYellowTriangleAttack(YellowTriangleEnemy yellowTriangle) : base(yellowTriangle)
    {
        effectPlayer = yellowTriangle.EffectPlayer;
        skillEffectName = yellowTriangle.skillEffectName;
    }

    public override void ExecuteInRangeMethod()
    {
        //List<BuffData> tmpData = enemy.HealthData.GrantedSkill.LootBuffFromDB(); // --> 스킬로 변경

        //for (int i = 0; i < tmpData.Count; i++)
        //{
        //    BaseBuff tmpBuff = enemy.LoadPlayer.BuffController.AddBuff(tmpData[i]);
        //    if (tmpBuff == null) continue;
        //    else storedBuff.Add(tmpBuff);
        //}

        // 버프 넣는 부분 스킬로 수정

        //BasicEffectPlayer effectPlayer = enemy.EffectMethod.ReturnEffectFromPool();
        //if (effectPlayer == null) return;

        //enemy.EffectPlayer = effectPlayer;
        //enemy.EffectPlayer.AddState(enemy.transform);
        //enemy.EffectPlayer.PlayEffect();
        loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);

        effectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(skillEffectName); // 이팩트 이름 추가
        if (effectPlayer == null) return;

        effectPlayer.Init(loadFollowEnemy.transform);
        effectPlayer.PlayEffect();

    }

    public override void ExecuteInOutsideMethod()
    {
        loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.OutRange);

        if (effectPlayer == null) return;
        effectPlayer.StopEffect();
    }

    public override void ReceiveOnDisable()
    {
        //ExecuteInRangeMethod();
        loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.OutRange); // 제거될 경우, 걸어둔 버프도 같이 제거

        // 여기에 SkillController에 존재하는 모든 스킬들을 없애주는 코드를 추가로 넣자



        if (effectPlayer != null)
        {
            effectPlayer.StopEffect();
            effectPlayer = null;
        }
    }
}
