using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponent : MonoBehaviour
{
    List<Collision2D> entity = new List<Collision2D>();
    Player player;
    AttackComponent attackComponent;

    [SerializeField]
    List<BasicSkill> loadSkill = new List<BasicSkill>();
    // <-- null이면 스킬 데이터를 플레이어에서 불러와서 사용, 아니면 저장되어 있는 객체에 playskill 실행해준다.
    // count 변수를 만들어서 0이면 리스트에서 삭제, 1 이상일 경우 하나씩 빼주면서 사용

    public EntityTag entityTag;

    private void Start()
    {
        player = GetComponent<Player>();
        attackComponent = GetComponent<AttackComponent>();
        player.collisionEnterAction += AddToList;
        player.collisionExitAction += RemoveToList;
        PlayManager.Instance.actionJoy.actionComponent.attackAction += PlayWhenAttackStart;
    }

    public void RemoveSkillFromLoad(BasicSkill skill)
    {
        loadSkill.Remove(skill);
    }

    public void AddSkillToLoad(BasicSkill skill)
    {
        loadSkill.Add(skill);
    }

    public void UseSkillInList()
    {
        for (int i = 0; i < loadSkill.Count; i++)
        {
            loadSkill[i].PlaySkill(player.rigid.velocity.normalized, entity);
        }
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

            skill.Init(transform, this);

            AddSkillToLoad(skill); // 먹어서 사용하는 스킬은 넣고
            player.SkillData.ResetSkill();
        }
        else // 스킬을 사용할 수 없는 경우 기본 스킬을 사용하게 한다.
        {
            BasicSkill normalSkill = GetSkillUsingType(player.NormalSkillData.Name);
            if (normalSkill == null) return;

            normalSkill.Init(transform, this); // 기본 스킬은 안 넣어도 될 듯
        }

        UseSkillInList(); // 리스트에 들어간 모든 오브젝트를 조건부로 실행해줌
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
