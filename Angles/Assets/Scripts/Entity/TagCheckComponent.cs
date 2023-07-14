using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagCheckComponent : MonoBehaviour
{
    [SerializeField]
    List<EntityTag> tags = new List<EntityTag>();
    public List<EntityTag> AbleTags
    {
        get { return tags; }
        set { tags = value; }
    }

    public bool IsAbleToCheck(Collision2D col)
    {
        col.gameObject.TryGetComponent(out Entity entity);
        if (entity == null || CheckTags(entity.InheritedTag) == false) return false;

        return true;
    }

    public bool CheckTags(EntityTag findTag)
    {
        for (int i = 0; i < tags.Count; i++)
        {
            if (tags[i] == findTag) return true;
        }

        return false;
    }
}
