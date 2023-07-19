using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffComponent : MonoBehaviour
{
    [SerializeField]
    List<BaseBuff> m_buffs;

    public void AddBuff(string name)
    {
        BaseBuff foundBuff = ObjectPooler.SpawnFromPool<BaseBuff>(name);

        m_buffs.Add(foundBuff);
        foundBuff.OnStart(gameObject);
    }

    public void RemoveBuff(string name)
    {
        BaseBuff foundBuff = m_buffs.Find(x => x.BuffName == name);

        if (foundBuff == null) return;

        m_buffs.Remove(foundBuff);
        foundBuff.OnEnd();
    }

    void BuffTick()
    {
        for (int i = 0; i < m_buffs.Count; i++)
        {
            m_buffs[i].Tick(Time.deltaTime);
            if(m_buffs[i].IsFinished)
            {
                m_buffs[i].OnEnd();
                m_buffs.Remove(m_buffs[i]);
            }
        }
    }


    private void Update()
    {
        BuffTick();
    }
}
