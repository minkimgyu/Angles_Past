using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffComponent : MonoBehaviour
{
    [SerializeField]
    List<BaseBuff> m_buffs;

    public bool AddBuff(string name) //--> ���� ȿ���� n�� �̻� ������ ���� --> buffData�� �Ѱ�����   
    {
        BuffData data = DatabaseManager.Instance.UtilizationDB.BuffDatas.Find(x => x.Name == name).CopyData();
        if (CheckBuffList(name) >= data.MaxCount) return false; // maxCount���� �� ���� ������ ������ �ִٸ� Return

        BaseBuff foundBuff = ObjectPooler.SpawnFromPool<BaseBuff>(name);

        foundBuff.Init(data); // ���� �ʱ�ȭ

        m_buffs.Add(foundBuff);
        foundBuff.OnStart(gameObject);

        return true;
    }

    public bool RemoveBuff(string name)
    {
        BaseBuff foundBuff = m_buffs.Find(x => x.Data.Name == name);
        if (foundBuff == null) return false;

        m_buffs.Remove(foundBuff);
        foundBuff.OnEnd();

        return true;
    }

    int CheckBuffList(string name)
    {
        int tmpCount = 0;

        for (int i = 0; i < m_buffs.Count; i++)
        {
            if (m_buffs[i].Data.Name == name) tmpCount++;
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


    private void Update()
    {
        BuffTick();
    }
}
