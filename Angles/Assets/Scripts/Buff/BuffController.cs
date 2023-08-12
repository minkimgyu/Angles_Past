using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFactory // �ϳ��� ������ --> ��ų ���丮�� ��������
{
    Dictionary<string, IBuff> storedBuffs;

    public void Init() // �̷� ������ ��ų ����
    {
        // �̷������� �ٸ� ������ ������ �� �ֵ��� ����
        //storedBuffs.Add();
    }

    public IBuff OrderBuff(Transform caster, BuffData data)
    {
        return storedBuffs[data.Name].CreateCopy(data);
    }
}

public class BuffController : MonoBehaviour
{
    [SerializeField]
    List<IBuff> m_buffs;

    BuffEffectComponent effectComponent;
    BuffFactory buffFactory;

    private void Awake()
    {
        buffFactory.Init();
    }

    private void Start()
    {
        effectComponent = GetComponentInChildren<BuffEffectComponent>();
    }

    public bool AddBuff(BuffData data) //--> ���� ȿ���� n�� �̻� ������ ���� --> buffData�� �Ѱ�����   
    {
        if (CheckBuffList(data) >= data.MaxCount) return false; // maxCount���� �� ���� ������ ������ �ִٸ� Return

        IBuff foundBuff = buffFactory.OrderBuff(transform, data);
        foundBuff.Init(data); // ���� �ʱ�ȭ
        foundBuff.OnStart(gameObject);

        return true;
    }  

    public bool RemoveBuff(BuffData data)
    {
        IBuff foundBuff = m_buffs.Find(x => x.ReturnBuffData() == data);
        if (foundBuff == null) return false;

        foundBuff.OnEnd();
        m_buffs.Remove(foundBuff);
        return true;
    }

    int CheckBuffList(BuffData data)
    {
        int tmpCount = 0;

        for (int i = 0; i < m_buffs.Count; i++)
        {
            if (data == m_buffs[i].ReturnBuffData()) tmpCount++;
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
