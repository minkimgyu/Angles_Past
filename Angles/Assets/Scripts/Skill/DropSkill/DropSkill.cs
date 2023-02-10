using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSkill : MonoBehaviour
{
    [SerializeField]
    SkillData dropSkillData;
    public SkillData DropSkillData
    {
        get
        {
            return dropSkillData;
        }
        set
        {
            dropSkillData = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().SkillData = DropSkillData;
            col.gameObject.GetComponent<BattleComponent>().PlayWhenGet();

            Destroy(gameObject);
        }
    }
}
