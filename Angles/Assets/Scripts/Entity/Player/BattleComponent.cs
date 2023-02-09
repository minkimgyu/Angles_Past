using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponent : MonoBehaviour
{
    public List<GameObject> entity;
    Player player;
    AttackComponent attackComponent;

    public EntityTag entityTag;

    public BasicSkill basicSkill;

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
        if (col.gameObject == null) return;

        entity.Add(col.gameObject);
        PlayWhenCollision();
    }

    void RemoveToList(Collision2D col) => entity.Remove(col.gameObject);
    
    void PlayWhenCollision()
    {
        if (player.PlayerMode != ActionMode.Attack) return;

        bool nowAttackEnemy = false;
        // 공격 시에만 적용

        for (int i = 0; i < entity.Count; i++)
        {
            if (entity[i].CompareTag(entityTag.ToString()))
            {
                if(nowAttackEnemy == false) nowAttackEnemy = true;
                entity[i].GetComponent<FollowComponent>().WaitFollow();
            }
        }

        if (nowAttackEnemy == true)
        {
            basicSkill.PlaySkillWhenCollision();
            attackComponent.QuickEndTask();
        }
    }

    void PlayWhenAttackStart(Vector2 dir)
    {
        Debug.Log(entity.Count);

        if (entity.Count != 0) // 현재 닿아있는 오브젝트가 존재할 때
        {
            Debug.Log(entity.Count);
            PlayWhenCollision();
        }

        basicSkill.PlaySkillWhenAttackStart();
    }

    void PlayWhenGet()
    {
        basicSkill.PlaySkillWhenGet();
    }

    private void OnDisable()
    {
        if (player == null) return;
        player.collisionEnterAction -= AddToList;
        player.collisionExitAction -= RemoveToList;
    }
}
