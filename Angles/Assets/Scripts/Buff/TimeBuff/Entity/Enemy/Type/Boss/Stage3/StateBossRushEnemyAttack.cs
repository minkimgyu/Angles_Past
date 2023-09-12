using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBossRushEnemyAttack : BaseState<BaseFollowEnemy.State>
{
    BossRushEnemy loadBossEnemy;
    SpriteRenderer spriteRenderer;

    float smoothness = 0.02f;

    Color fadeColor;
    Color nowColor;

    float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
    float increment = 0;
    bool nowReversal = false;
    bool canSwitchColor = false;

    public StateBossRushEnemyAttack(BossRushEnemy bossEnemy)
    {
        loadBossEnemy = bossEnemy;
        spriteRenderer = loadBossEnemy.rushDirectionIcon.GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public override void ReceiveCollisionEnter(Collision2D collision)
    {
        loadBossEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.Contact);
    }

    public override void OperateEnter()
    {
        Vector3 tmpDir = (loadBossEnemy.LoadPlayer.transform.position - loadBossEnemy.transform.position).normalized;
        loadBossEnemy.DashComponent.PlayDash(tmpDir, loadBossEnemy.AttackThrust.IntervalValue, loadBossEnemy.AttackDuration.IntervalValue, true);

        increment = smoothness / (loadBossEnemy.AttackDuration.IntervalValue / 2); //The amount of change to apply.
        canSwitchColor = true;

        loadBossEnemy.rushDirectionIcon.gameObject.SetActive(true);

        // 방향 벡터를 각도로 변환합니다.
        float angle = Mathf.Atan2(tmpDir.y, tmpDir.x) * Mathf.Rad2Deg;
        loadBossEnemy.rushDirectionIcon.rotation = Quaternion.Euler(0, 0, angle);

        fadeColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        nowColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
    }

    public override void OperateExit()
    {
        loadBossEnemy.rushDirectionIcon.gameObject.SetActive(false);
    }

    public override void OperateUpdate()
    {
        //여기서 이팩트 조절하기
        if (loadBossEnemy.DashComponent.NowFinish == true)
        {
            loadBossEnemy.DashComponent.QuickEndTask();
            loadBossEnemy.RevertToPreviousState();
        }

        if (canSwitchColor == false) return;

        if(nowReversal == false)
        {
            spriteRenderer.color = Color.Lerp(fadeColor, nowColor, progress);
            progress += increment;

            if(progress >= 1)
            {
                progress = 0;
                nowReversal = true;
            }
        }
        else
        {
            spriteRenderer.color = Color.Lerp(nowColor, fadeColor, progress);
            progress += increment;

            if (progress >= 1)
            {
                progress = 0;
                nowReversal = false;
                canSwitchColor = false;
            }
        }
    }
}
