using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    [SerializeField]
    List<BaseBuff> m_buffs;

<<<<<<< Updated upstream:Angles/Assets/Scripts/Buff/BuffController.cs
    BuffEffectComponent effectComponent;
    BuffFactory buffFactory;

    private void Start()
    {
        effectComponent = GetComponentInChildren<BuffEffectComponent>();
    }

    public bool AddBuff(string name) //--> ���� ȿ���� n�� �̻� ������ ���� --> buffData�� �Ѱ�����   
=======
    public BaseBuff AddBuff(BuffData data) //--> ���� ȿ���� n�� �̻� ������ ���� --> buffData�� �Ѱ�����   
>>>>>>> Stashed changes:Angles/Assets/Scripts/Buff/BuffComponent.cs
    {
        BaseBuff orderedBuff = buffFactory.Order(name);

        if (CheckBuffList(name) >= orderedBuff.MaxCount) return false; // maxCount���� �� ���� ������ ������ �ִٸ� Return
        orderedBuff.OnStart(gameObject);

<<<<<<< Updated upstream:Angles/Assets/Scripts/Buff/BuffController.cs
        return true;
=======
        foundBuff.Init(data); // ���� �ʱ�ȭ

        m_buffs.Add(foundBuff);
        foundBuff.OnStart(gameObject);

        return foundBuff;
>>>>>>> Stashed changes:Angles/Assets/Scripts/Buff/BuffComponent.cs
    }  

    public bool RemoveBuff(string name)
    {
<<<<<<< Updated upstream:Angles/Assets/Scripts/Buff/BuffController.cs
        BaseBuff orderedBuff = m_buffs.Find(x => x.Name == name);
        if (orderedBuff == null) return false;
=======
        BaseBuff foundBuff = m_buffs.Find(x => x == buff);
        if (foundBuff == null) return false;

        m_buffs.Remove(foundBuff);
        foundBuff.OnEnd();
>>>>>>> Stashed changes:Angles/Assets/Scripts/Buff/BuffComponent.cs

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
