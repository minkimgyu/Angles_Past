using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSkill : MonoBehaviour
{
    SkillType skill;
    public SkillType Skill
    {
        get
        {
            return skill;
        }
        set
        {
            skill = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().ResetSkill(Skill);
        }
    }
}
