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
        //List<BuffData> tmpData = enemy.HealthData.GrantedSkill.LootBuffFromDB(); // --> ��ų�� ����

        //for (int i = 0; i < tmpData.Count; i++)
        //{
        //    BaseBuff tmpBuff = enemy.LoadPlayer.BuffController.AddBuff(tmpData[i]);
        //    if (tmpBuff == null) continue;
        //    else storedBuff.Add(tmpBuff);
        //}

        // ���� �ִ� �κ� ��ų�� ����

        //BasicEffectPlayer effectPlayer = enemy.EffectMethod.ReturnEffectFromPool();
        //if (effectPlayer == null) return;

        //enemy.EffectPlayer = effectPlayer;
        //enemy.EffectPlayer.AddState(enemy.transform);
        //enemy.EffectPlayer.PlayEffect();
        loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);

        effectPlayer = ObjectPooler.SpawnFromPool<BasicEffectPlayer>(skillEffectName); // ����Ʈ �̸� �߰�
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
        loadFollowEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.OutRange); // ���ŵ� ���, �ɾ�� ������ ���� ����

        // ���⿡ SkillController�� �����ϴ� ��� ��ų���� �����ִ� �ڵ带 �߰��� ����



        if (effectPlayer != null)
        {
            effectPlayer.StopEffect();
            effectPlayer = null;
        }
    }
}
