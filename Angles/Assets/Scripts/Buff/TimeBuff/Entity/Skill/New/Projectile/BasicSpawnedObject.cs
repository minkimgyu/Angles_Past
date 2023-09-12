using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� �ð��� ������ ������ �ı��Ǵ� ������Ʈ
///  �ı��� �� �Ǹ� ��� ���� ���� �����Ƿ� ��������
/// </summary>
abstract public class BasicSpawnedObject : MonoBehaviour
{
    protected SkillController m_skillController;
    public SkillController SkillController { get { return m_skillController; } }

    protected GrantedSkill grantedSkill;

    protected EntityTag[] hitTargetTag;

    protected bool IsTarget(GameObject target, EntityTag[] tags)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (target.CompareTag(tags[i].ToString()) == true)
            {
                return true;
            }
        }

        return false;
    }

    private void Update() => DoUpdate();

    protected virtual void DoUpdate() { }

    protected virtual void Awake()
    {
        m_skillController = GetComponent<SkillController>();
    }

    public void Inintialize(float disableTime, string[] skillNames)
    {
        Invoke("DisableObject", disableTime);
        grantedSkill = new GrantedSkill(skillNames);
        grantedSkill.LootSkillFromDB(m_skillController);
        m_skillController.UseSkill(BaseSkill.UseConditionType.Init); // --> �̷� ������ ��ų�� �۵�
    }

    // �ڽ� ��ü���� �߰� ����
    public virtual void ResetObject(Transform caster, Vector3 pos) 
    {
    }

    public virtual void ResetObject(Transform caster, float rotation, float distanceFromCaster, SpawnRotationBall spawnRotationBall)
    {
    }

    public virtual void ResetObject(Transform tr) 
    {
    }

    public virtual void ResetObject(Vector3 pos) 
    {
    }

    public virtual void ResetObject(Vector3 pos, float rotation) 
    {
    }

    public virtual void DisableObject()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }
}

/// <summary>
/// �߻� ������ ������Ʈ���� ��ӽ�Ű��
/// </summary>
public interface IProjectile
{
    //public DashComponent DashComponent { get; set; }

    public void Shoot(Vector2 dir, float thrust); // �̰ɷ� �߻� ��� ����
}

/// <summary>
/// �ٸ� ������Ʈ�� �浹 ��, �ı��Ǹ� ��ų�� ����ϴ� ������Ʈ�� ��ӽ�Ű��
/// ex) �Ѿ�
/// </summary>
public class ContactableObject : BasicSpawnedObject
{
    protected ContactComponent m_contactComponent; // --> ������, ���� ��� ����� ������ �־�����

    protected override void Awake()
    {
        base.Awake();
        m_contactComponent = GetComponent<ContactComponent>();
    }

    void CheckFinish(Collision2D col)
    {
        bool canFinish = IsTarget(col.gameObject, hitTargetTag);
        if (canFinish == false) return;

        DisableObject();
    }

    protected void OnCollisionEnter2D(Collision2D col) // �浹 �� ���� ��ȯ
    {
        m_contactComponent.CallWhenCollisionEnter(col);
        m_skillController.UseSkill(BaseSkill.UseConditionType.Contact); // --> �̷� ������ ��ų�� �۵�
        CheckFinish(col);
    }

    protected void OnCollisionExit2D(Collision2D col)
    {
        m_contactComponent.CallWhenCollisionExit(col);
    }
}

/// <summary>
/// ���� �ð��� ������ ������ �ı��Ǵ� ������Ʈ
/// </summary>
//public class SelfDestructingObject : BasicSpawnedObject
//{
//    protected float disableTime;

//    public override void ResetObject(Transform caster)
//    {
//        Invoke("DisableObject", disableTime);
//    }

//    public override void ResetObject(Vector3 pos)
//    {
//        Invoke("DisableObject", disableTime);
//    }

//    protected override void OnDisable()
//    {
//        CancelInvoke();
//        base.OnDisable();
//    }
//}



/// <summary>
/// �Ѿ˰� ���� ������Ʈ
/// 
/// �浹�ϸ� �������� �ְ� �μ����� Ư�� �������� ����
/// </summary>
