using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    [SerializeReference]
    List<BaseBuff> m_buffs = new List<BaseBuff>();

    public bool AddBuff(string name) //--> ���� ȿ���� n�� �̻� ������ ���� --> buffData�� �Ѱ�����   
    {
        BaseBuff orderedBuff = BuffFactory.Order(name);

        if (CheckBuffList(name) >= orderedBuff.MaxCount) return false; // maxCount���� �� ���� ������ ������ �ִٸ� Return
        orderedBuff.OnStart(gameObject);
        m_buffs.Add(orderedBuff);

        return true;
    }  

    public bool RemoveBuff(string name)
    {
        BaseBuff orderedBuff = m_buffs.Find(x => x.Name == name);
        if (orderedBuff == null) return false;

        orderedBuff.OnEnd();
        m_buffs.Remove(orderedBuff);
        return true;
    }

    int CheckBuffList(string name)
    {
        int tmpCount = 0;

        for (int i = 0; i < m_buffs.Count; i++)
        {
            if (name == m_buffs[i].Name) tmpCount++;
        }

        return tmpCount;
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

    private void OnDisable()
    {
        if (m_buffs.Count <= 0) return;

        for (int i = 0; i < m_buffs.Count; i++)
        {
            m_buffs[i].OnEnd();
            m_buffs.Remove(m_buffs[i]);
        }
    }

    private void Update()
    {
        BuffTick();
    }
}
