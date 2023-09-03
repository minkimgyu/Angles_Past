using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootingItemComponent : MonoBehaviour
{
    public void LootingItem(Collider2D col, SkillController skillController)
    {
        if (col.gameObject.CompareTag("DropItem"))
        {
            DropSkill dropSkill = col.GetComponent<DropSkill>();
            SoundManager.Instance.PlaySFX(transform.position, "GetItem", 0.1f);

            Debug.Log(dropSkill.ReturnSkill());
            skillController.AddSkillToList(dropSkill.ReturnSkill());
        }
    }
}
