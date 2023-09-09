using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TriggerComponent : MonoBehaviour
{
    [SerializeField]
    List<GameObject> oldGos = new List<GameObject>();

    [SerializeField]
    List<GameObject> Gos = new List<GameObject>();

    public void CallWhenTriggerEnter(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
            Gos.Add(col.gameObject);
    }

    public void CallWhenTriggerExit(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
            Gos.Remove(col.gameObject);
    }

    public List<GameObject> ReturnTriggeredObjects(bool isEnter)
    {
        List<GameObject> newGos = new List<GameObject>();

        if (isEnter)
        {
            for (int i = 0; i < Gos.Count; i++)
            {
                if (!oldGos.Contains(Gos[i]))
                {
                    newGos.Add(Gos[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < oldGos.Count; i++)
            {
                if (!Gos.Contains(oldGos[i]))
                {
                    newGos.Add(oldGos[i]);
                }
            }
        }

        oldGos.Clear();
        for (int i = 0; i < Gos.Count; i++)
        {
            oldGos.Add(Gos[i]);
        }

        return newGos;
    }
}
