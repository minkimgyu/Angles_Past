using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSupportData
{
    public List<Vector3> contactPos;
    public List<Entity> contactEntity;
    public Vector3 playerDir;
    public Entity player;

    public SkillSupportData(Entity player, Vector3 playerDir, List<Entity> contactEntity, List<Vector3> contactPos)
    {
        this.player = player;
        this.playerDir = playerDir;
        this.contactEntity = contactEntity;
        this.contactPos = contactPos;
    }
}

[System.Serializable]
public struct ContactData
{
    public GameObject go;
    public Vector3 pos;

    public ContactData(GameObject go, Vector3 pos)
    {
        this.go = go;
        this.pos = pos;
    }
}

public class BattleComponent : BasicBattleComponent
{
    public List<ContactData> contactDatas = new List<ContactData>();

    Entity player;
    AttackComponent attackComponent;

    [SerializeField]
    SkillData skillData;
    public SkillData SkillData { get { return skillData; } set { skillData = value; } }
    public EntityTag contactTag;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Entity>();
        attackComponent = GetComponent<AttackComponent>();
        PlayManager.Instance.actionJoy.actionComponent.attackAction += PlayWhenAttackStart;
    }

    #region 주변의 적 체크

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(contactTag.ToString()) == false) return;

        ContactData contactData = new ContactData(col.gameObject, col.contacts[0].point);
        contactDatas.Add(contactData);
        PlayWhenCollision();
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(contactTag.ToString()) == false) return;

        for (int i = 0; i < contactDatas.Count; i++)
        {
            if(contactDatas[i].go == col.gameObject)
            {
                contactDatas.RemoveAt(i);
                return;
            }
        }
    }

    #endregion

    SkillSupportData ReturnContactEntity()
    {
        List<Vector3> pos = new List<Vector3>();
        List<Entity> entity = new List<Entity>();

        Vector3 dir = player.rigid.velocity.normalized;

        for (int i = 0; i < contactDatas.Count; i++)
        {
           pos.Add(contactDatas[i].pos);
           entity.Add(contactDatas[i].go.GetComponent<Entity>());
        }

        SkillSupportData supportData = new SkillSupportData(player, dir, entity, pos);
        return supportData;
    }

    protected override void UseSkillInList(SkillUseType useType)
    {
        SkillSupportData supportData = ReturnContactEntity();

        for (int i = 0; i < loadSkillDatas.Count; i++)
        {
            if (loadSkillDatas[i].CanUseSkill(useType) == false) continue;

            BasicSkill skill = GetSkillUsingName(loadSkillDatas[i].Name.ToString(), transform.position, Quaternion.identity);
            if (skill == null) continue;

            skill.PlaySkill(supportData);
            loadSkillDatas[i].UseSkill(loadSkillDatas);
        }
    }

    bool NowContactEnemy()
    {
        return contactDatas.Count > 0;
    }

    protected override void UseSkill(SkillUseType skillUseType)
    {
        SkillData data;

        // 스킬을 사용할 수 있는 경우 스킬 사용
        if (SkillData.CanUseSkill(skillUseType) == true)
        {
            data = SkillData.CopyData();
            SkillData.ResetSkill(); // 플레이어 보유 스킬은 리셋

            loadSkillDatas.Add(data);
            UseSkillInList(skillUseType); // 리스트에 들어간 모든 오브젝트를 조건부로 실행해줌
        }
        else if(NormalSkillData.CanUseSkill(skillUseType) == true) // 스킬을 사용할 수 없는 경우 기본 스킬을 사용하게 한다.
        {
            data = NormalSkillData.CopyData();

            loadSkillDatas.Add(data);
            UseSkillInList(skillUseType); // 리스트에 들어간 모든 오브젝트를 조건부로 실행해줌
        }
    }

    //getfix이면 먹을 경우 재사용
    
    void PlayWhenCollision()
    {
        if (player.PlayerMode != ActionMode.Attack) return;

        UseSkill(SkillUseType.Contact);

        attackComponent.QuickEndTask(); // 공격 리셋해주기
    }

    void PlayWhenAttackStart(Vector2 dir, ForceMode2D forceMode)
    {
        if (NowContactEnemy() == true) PlayWhenCollision(); // 접촉 시, 충돌 함수 실행
    }

    public void PlayWhenGet()
    {
        UseSkill(SkillUseType.Get);
    }

    private void OnDisable()
    {
        if (PlayManager.Instance != null)
            PlayManager.Instance.actionJoy.actionComponent.attackAction -= PlayWhenAttackStart;
    }
}
