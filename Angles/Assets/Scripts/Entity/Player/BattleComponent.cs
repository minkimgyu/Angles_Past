using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponent : MonoBehaviour
{
    public List<Collision2D> entity = new List<Collision2D>();
    Player player;
    AttackComponent attackComponent;

    public EntityTag entityTag;

    private void Start()
    {
        player = GetComponent<Player>();
        attackComponent = GetComponent<AttackComponent>();
        player.collisionEnterAction += AddToList;
        player.collisionExitAction += RemoveToList;
        PlayManager.Instance.actionJoy.actionComponent.attackAction += PlayWhenAttackStart;
    }

    void AddToList(Collision2D col)
    {
        if (col.gameObject.CompareTag(entityTag.ToString()) != true) return;

        entity.Add(col);
        PlayWhenCollision();
    }

    void RemoveToList(Collision2D col) => entity.Remove(col);

    BasicSkill GetSkillUsingType(SkillName skillName)
    {
        BasicSkill skill = ObjectPooler.SpawnFromPool(skillName.ToString()).GetComponent<BasicSkill>();
        return skill;
    }

    bool NowContactEnemy()
    {
        return entity.Count > 0;
    }

    void UseSkill(SkillUseType skillUseType)
    {
        bool canUseSkill = player.SkillData.CanUseSkill(skillUseType);

        // 스킬을 사용할 수 있는 경우 스킬 사용
        if (canUseSkill == true)
        {
            BasicSkill skill = GetSkillUsingType(player.SkillData.Name);
            if (skill == null) return;

            //Debug.Log(transform.position);

            skill.Init(transform, player.rigid.velocity, entity);
            player.SkillData.ResetSkill();
        }
        else // 스킬을 사용할 수 없는 경우 기본 스킬을 사용하게 한다.
        {
            BasicSkill normalSkill = GetSkillUsingType(player.NormalSkillData.Name);
            if (normalSkill == null) return;

            //Debug.Log(transform.position);

            //for (int i = 0; i < entity.Count; i++)
            //{
            //    print(entity[i].gameObject.name);
            //}

            normalSkill.Init(transform, player.rigid.velocity, entity);
        }
    }
    
    void PlayWhenCollision()
    {
        if (NowContactEnemy() == false) return; // 적과 접촉하고 있지 않은 경우 리턴
        if (player.PlayerMode != ActionMode.Attack) return;

        UseSkill(SkillUseType.Contact);

        attackComponent.QuickEndTask(); // 공격 리셋해주기
    }

    void PlayWhenAttackStart(Vector2 dir)
    {
        PlayWhenCollision(); // 한번 체크

        UseSkill(SkillUseType.Start);
    }

    public void PlayWhenGet()
    {
        UseSkill(SkillUseType.Get);
    }

    private void OnDisable()
    {
        if (player == null) return;
        player.collisionEnterAction -= AddToList;
        player.collisionExitAction -= RemoveToList;
    }
}
