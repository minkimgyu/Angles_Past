using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 일정 시간이 지나면 스스로 파괴되는 오브젝트
///  파괴가 안 되면 계속 쌓일 수도 있으므로 삭제해줌
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
        m_skillController.UseSkill(BaseSkill.UseConditionType.Init); // --> 이런 식으로 스킬로 작동
    }

    // 자식 객체에서 추가 구현
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
/// 발사 가능한 오브젝트한테 상속시키기
/// </summary>
public interface IProjectile
{
    //public DashComponent DashComponent { get; set; }

    public void Shoot(Vector2 dir, float thrust); // 이걸로 발사 기능 구현
}

/// <summary>
/// 다른 오브젝트와 충돌 시, 파괴되며 스킬을 사용하는 오브젝트에 상속시키기
/// ex) 총알
/// </summary>
public class ContactableObject : BasicSpawnedObject
{
    protected ContactComponent m_contactComponent; // --> 데미지, 범위 등등 공통된 변수만 넣어주자

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

    protected void OnCollisionEnter2D(Collision2D col) // 충돌 시 상태 변환
    {
        m_contactComponent.CallWhenCollisionEnter(col);
        m_skillController.UseSkill(BaseSkill.UseConditionType.Contact); // --> 이런 식으로 스킬로 작동
        CheckFinish(col);
    }

    protected void OnCollisionExit2D(Collision2D col)
    {
        m_contactComponent.CallWhenCollisionExit(col);
    }
}

/// <summary>
/// 일정 시간이 지나면 스스로 파괴되는 오브젝트
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
/// 총알과 같은 오브젝트
/// 
/// 충돌하면 데미지를 주고 부서지며 특정 방향으로 날라감
/// </summary>
