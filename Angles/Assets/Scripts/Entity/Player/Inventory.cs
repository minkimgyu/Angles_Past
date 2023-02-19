using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Player player;
    BattleComponent battleComponent;

    private void Start()
    {
        player = GetComponent<Player>();
        battleComponent = GetComponent<BattleComponent>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("DropItem"))
        {
            DropSkill dropSkill = col.GetComponent<DropSkill>();
            battleComponent.SkillData = dropSkill.DropSkillData;
            battleComponent.PlayWhenGet();
            Destroy(dropSkill.gameObject);
        }
    }
}
