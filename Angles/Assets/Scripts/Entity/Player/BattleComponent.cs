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

    #region �ֺ��� �� üũ

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

            loadBasicSkills.Add(skill);

            skill.PlaySkill(supportData, this);
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

        bool nowRun = CheckCountUp(SkillData, skillUseType); // ī��Ʈ ���� ���⼭ �����Ŵ
        if (nowRun == true) return;

        // ��ų�� ����� �� �ִ� ��� ��ų ���
        if (SkillData.CanUseSkill(skillUseType) == true)
        {
            data = SkillData.CopyData();
            SkillData.ResetSkill(); // �÷��̾� ���� ��ų�� ����

            AddToDataList(data, skillUseType);
        }
        else if(NormalSkillData.CanUseSkill(skillUseType) == true) // ��ų�� ����� �� ���� ��� �⺻ ��ų�� ����ϰ� �Ѵ�.
        {
            data = NormalSkillData.CopyData();
            AddToDataList(data, skillUseType);
        }
    }

    public void RemoveSkillInList(SkillData skillData)
    {
        loadSkillDatas.Remove(skillData);
    }

    int CheckSkillList(SkillName skillName)
    {
        for (int i = 0; i < loadBasicSkills.Count; i++)
        {
            if (loadBasicSkills[i].SkillName == skillName)
            {
                return i;
            }
        }

        return -1;
    }

    SkillData ReturnSkillData(SkillName skillName)
    {
        for (int i = 0; i < loadSkillDatas.Count; i++)
        {
            if (loadSkillDatas[i].Name == skillName)
            {
                return loadSkillDatas[i];
            }
        }

        return null;
    }

    bool CheckCountUp(SkillData data, SkillUseType skillUseType)
    {
        print("CheckCountUp1112" + skillUseType);

        if (skillUseType != SkillUseType.Get) return false;

        if (data.Usage == SkillUsage.CountUp)
        {
            print("CheckCountUp");
            SkillData loadData = ReturnSkillData(data.Name);
            if (loadData == null) return false;

            loadData.CountUp();
            SkillData.ResetSkill(); // �÷��̾� ���� ��ų�� ����
            return true;
        }
        else if (data.Usage == SkillUsage.Overlap)
        {
            print("Overlap");
            int index = CheckSkillList(data.Name);
            if (index == -1) return false;

            print(index);

            loadBasicSkills[index].PlayAddition(); // �߰� �Լ� ����
            SkillData.ResetSkill(); // �÷��̾� ���� ��ų�� ����
            return true;
        }

        return false;
    }
    
    void AddToDataList(SkillData data, SkillUseType skillUseType)
    {
        loadSkillDatas.Add(data); // ��ų ����Ʈ�� ������ �߰����ش�.
        UseSkillInList(skillUseType); // ����Ʈ�� �� ��� ������Ʈ�� ���Ǻη� ��������
    }

    void PlayWhenCollision()
    {
        if (player.PlayerMode != ActionMode.Attack) return;

        UseSkill(SkillUseType.Contact);

        attackComponent.QuickEndTask(); // ���� �������ֱ�
    }

    void PlayWhenAttackStart(Vector2 dir, ForceMode2D forceMode)
    {
        if (NowContactEnemy() == true) PlayWhenCollision(); // ���� ��, �浹 �Լ� ����
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
